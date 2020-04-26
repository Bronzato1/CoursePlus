﻿using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooks();
    }
}
