﻿
namespace MovieStore.Api.DTOs
{
    public class PageDTO
    {

        private readonly int _maxPageSize = 40;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }

    }
}
