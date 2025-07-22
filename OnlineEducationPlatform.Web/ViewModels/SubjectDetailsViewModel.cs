﻿namespace OnlineEducationPlatform.Web.ViewModels
{
    public class SubjectDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Class>? Class { get; set; }
        public IEnumerable<DateTime>? AddedDate { get; set; }
    }
}
