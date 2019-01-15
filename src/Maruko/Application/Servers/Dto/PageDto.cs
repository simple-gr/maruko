﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maruko.Application.Servers.Dto
{
    public abstract class PageDto
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [Range(1, 2147483647)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 一页数据总数
        /// </summary>
        [Range(1, 2147483647)]
        public int PageSize { get; set; } = 20;
    }
}
