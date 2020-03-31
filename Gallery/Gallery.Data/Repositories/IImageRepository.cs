﻿using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public interface IImageRepository : IRepository<Image>
	{
		Image Get(string title);
		bool Delete(string title);
	}
}