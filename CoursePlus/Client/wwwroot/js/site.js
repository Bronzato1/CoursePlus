function nightMode()
{
    // For Night mode
    if (!('localStorage' in window)) return;
    var nightMode = localStorage.getItem('gmtNightMode');
    if (nightMode) {
        document.documentElement.className += ' night-mode';
    }

    // Feature test
    if (!('localStorage' in window)) return;

    // Get our newly insert toggle
    var nightMode = document.querySelector('#night-mode');
    if (!nightMode) return;

    // When clicked, toggle night mode on or off
    nightMode.addEventListener('click', function (event) {
        event.preventDefault();
        document.documentElement.classList.toggle('night-mode');
        if (document.documentElement.classList.contains('night-mode')) {
            localStorage.setItem('gmtNightMode', true);
            return;
        }
        localStorage.removeItem('gmtNightMode');
    }, false);
}

function bootstrapSelect()
{
    $('.selectpicker').selectpicker();
}

window.OnBlazorReady = () =>
{
    nightMode();
    bootstrapSelect();
}

window.OpenBase64ImageInNewTab = (data) =>
{
    var image = new Image();
    image.src = "data:image/jpg;base64," + data;
    var w = window.open("");
    w.document.write(image.outerHTML);
}

window.Selectpicker = (elm, value) =>
{
    $(elm).selectpicker('val', value);
}

window.QuillFunctions =
{
    createQuill: function (quillElement) {
        var options = {
            debug: 'info',
            modules: {
                toolbar: '#quill-toolbar'
            },
            placeholder: 'Description of the playlist...',
            readOnly: false,
            theme: 'snow'
        };
        // set quill at the object we can call
        // methods on later
        new Quill(quillElement, options);
    },
    getQuillContent: function (quillControl) {
        // Delta format
        return JSON.stringify(quillControl.__quill.getContents());
    },
    getQuillText: function (quillControl) {
        // Text format
        return quillControl.__quill.getText();
    },
    getQuillHTML: function (quillControl) {
        // HTML format
        return quillControl.__quill.root.innerHTML;
    },
    loadQuillContent: function (quillControl, quillContent) {
        // From Delta format
        content = JSON.parse(quillContent);
        return quillControl.__quill.setContents(content, 'api');
    },
    loadQuillContentFromHTML: function (quillControl, htmlContent) {
        // From HTML format
        const delta = quillControl.__quill.clipboard.convert(htmlContent);
        return quillControl.__quill.setContents(delta, 'api');
    },
    notifyQuillChanges: function (quillControl, dotnetHelper) {
        quillControl.__quill.on('text-change', function (delta, oldDelta, source) {
            dotnetHelper.invokeMethodAsync('QuillContentChanged');
        });
    }
};

window.ResetSticky = (element) =>
{
    window.setTimeout(triggerResizeEvent, 100);
    
    window.onscroll = function ()
    {
        var B = document.body; //IE 'quirks'
        var D = document.documentElement; //IE with doctype
        D = (D.clientHeight) ? D : B;

        if (D.scrollTop == 0)
            window.setTimeout(triggerResizeEvent, 100);
    }
    
    function triggerResizeEvent()
    {
        var resizeNeeded = !$(element).hasClass('uk-active');
        if (resizeNeeded)
        {
            var el = document;
            var event = document.createEvent('HTMLEvents');
            event.initEvent('resize', true, false);
            el.dispatchEvent(event);
        }
    }
}

window.Timer =
{
    initialize: function (dotNetObject)
    {
        // States
        // ------
        // 1: Started
        // 2: Paused
        // 3: Resumed
        // 4: Elapsed

        //circle start
        let progressBar = document.querySelector('.e-c-progress');
        let indicator = document.getElementById('e-indicator');
        let pointer = document.getElementById('e-pointer');
        let length = Math.PI * 2 * 100;

        progressBar.style.strokeDasharray = length;

        function update(value, timePercent)
        {
            var offset = - length - length * value / (timePercent);
            progressBar.style.strokeDashoffset = offset;
            pointer.style.transform = `rotate(${360 * value / (timePercent)}deg)`;
        };
        //circle ends

        const displayOutput = document.querySelector('.display-remain-time')
        const pauseBtns = document.getElementsByClassName('startStopTimer');
        const setterBtns = document.querySelectorAll('button[data-setter]');

        let intervalTimer;
        let timeLeft;
        let wholeTime = 0.5 * 60; // manage this to set the whole time 
        let isPaused = false;
        let isStarted = false;


        update(wholeTime, wholeTime); //refreshes progress bar
        displayTimeLeft(wholeTime);

        function changeWholeTime(seconds)
        {
            if ((wholeTime + seconds) > 0)
            {
                wholeTime += seconds;
                update(wholeTime, wholeTime);
            }
        }

        for (var i = 0; i < setterBtns.length; i++)
        {
            setterBtns[i].addEventListener("click", function (event) {
                var param = this.dataset.setter;
                switch (param) {
                    case 'minutes-plus':
                        changeWholeTime(1 * 60);
                        break;
                    case 'minutes-minus':
                        changeWholeTime(-1 * 60);
                        break;
                    case 'seconds-plus':
                        changeWholeTime(1);
                        break;
                    case 'seconds-minus':
                        changeWholeTime(-1);
                        break;
                }
                displayTimeLeft(wholeTime);
            });
        }

        function timer(seconds)
        {
            //counts time, takes seconds
            let remainTime = Date.now() + (seconds * 1000);
            displayTimeLeft(seconds);

            intervalTimer = setInterval(function () {
                timeLeft = Math.round((remainTime - Date.now()) / 1000);
                if (timeLeft < 0) {
                    clearInterval(intervalTimer);
                    isStarted = false;
                    setterBtns.forEach(function (btn) {
                        btn.disabled = false;
                        btn.style.opacity = 1;
                    });
                    displayTimeLeft(wholeTime);

                    Array.from(pauseBtns).forEach((el) => {
                        el.classList.remove('pause');
                        el.classList.add('play');
                    });

                    dotNetObject.invokeMethodAsync('StateChanged', 4); // Elapsed
                    return;
                }
                displayTimeLeft(timeLeft);
            }, 1000);
        }

        function pauseTimer(event)
        {
            if (isStarted === false) {
                timer(wholeTime);
                isStarted = true;

                Array.from(pauseBtns).forEach((el) => {
                    el.classList.remove('play');
                    el.classList.add('pause');
                });

                setterBtns.forEach(function (btn) {
                    btn.disabled = true;
                    btn.style.opacity = 0.5;
                });
                dotNetObject.invokeMethodAsync('StateChanged', 1); // Started
            } else if (isPaused) {
                Array.from(pauseBtns).forEach((el) => {
                    el.classList.remove('play');
                    el.classList.add('pause');
                });
                timer(timeLeft);
                isPaused = isPaused ? false : true;
                dotNetObject.invokeMethodAsync('StateChanged', 3); // Resumed
            } else {
                Array.from(pauseBtns).forEach((el) => {
                    el.classList.remove('pause');
                    el.classList.add('play');
                });
                clearInterval(intervalTimer);
                isPaused = isPaused ? false : true;
                dotNetObject.invokeMethodAsync('StateChanged', 2); // Paused
            }
        }

        function displayTimeLeft(timeLeft)
        {
            //displays time on the input
            let minutes = Math.floor(timeLeft / 60);
            let seconds = timeLeft % 60;
            let displayString = `${minutes < 10 ? '0' : ''}${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
            displayOutput.textContent = displayString;
            update(timeLeft, wholeTime);
        }

        Array.from(pauseBtns).forEach((el) => {
           el.addEventListener('click', pauseTimer);
        });
         
    }
}

window.History =
{
    goBack: function () {
        window.history.go(-1);
    }
}

window.Player =
{
    initialize: function () {
        const player = new Plyr('#player');
        window.player = player;
    },
    loadYouTubeVideo: function (videoId)
    {
        window.player.source = {
            type: 'video',
            sources: [
                {
                    src: videoId,
                    provider: 'youtube'
                }
            ]
        };
    }
}

