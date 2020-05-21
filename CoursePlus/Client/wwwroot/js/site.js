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

window.onBlazorReady = () =>
{
    nightMode();
    bootstrapSelect();
}

window.openBase64ImageInNewTab = (data) =>
{
    var image = new Image();
    image.src = "data:image/jpg;base64," + data;
    var w = window.open("");
    w.document.write(image.outerHTML);
}

window.selectpicker = (elm, value) =>
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
            placeholder: 'Description of the course...',
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

window.resetSticky = (element) =>
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

window.History =
{
    goBack: function () {
        window.history.go(-1);
    }
};