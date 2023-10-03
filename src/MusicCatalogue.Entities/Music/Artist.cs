﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Music
{
    [ExcludeFromCodeCoverage]
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

#pragma warning disable CS8618
        public ICollection<Album> Albums { get; set; }
#pragma warning restore CS8618
    }
}
