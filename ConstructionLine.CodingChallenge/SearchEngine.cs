using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private Dictionary<Guid, SizeCount> _sizeCount;
        private Dictionary<Guid, ColorCount> _colorCount;
        private List<Shirt> _matches;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _sizeCount = Size.All.Select(sze => new SizeCount() { Size = sze }).ToDictionary(color => color.Size.Id);
            _colorCount = Color.All.Select(clr => new ColorCount() { Color = clr }).ToDictionary(color => color.Color.Id);
            _matches = new List<Shirt>();
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options == null || options.Colors == null || options.Sizes == null)
            {
                throw new ArgumentNullException("SearchOptions or one of its properties are null");
            }

            var matchedShirts = (from shirt in _shirts
                                 where (!options.Colors.Any() || options.Colors.Any(clr => clr == shirt.Color)) &&
                                       (!options.Sizes.Any() || options.Sizes.Any(sze => sze == shirt.Size))
                                 select shirt).ToList();

            var sizeCounts = matchedShirts.GroupBy(shirt => shirt.Size).Select(shirtGrp => new SizeCount() { Size = shirtGrp.Key, Count = shirtGrp.Count() }).ToList();
            sizeCounts.AddRange(Size.All.Where(size => !sizeCounts.Any(sze => sze.Size == size)).Select(sze => new SizeCount { Size = sze, Count = 0 }).ToList());

            var colorCounts = matchedShirts.GroupBy(shirt => shirt.Color).Select(colorGrp => new ColorCount() { Color = colorGrp.Key, Count = colorGrp.Count() }).ToList();
            colorCounts.AddRange(Color.All.Where(color => !colorCounts.Any(clr => clr.Color == color)).Select(clr => new ColorCount { Color = clr, Count = 0 }).ToList());

            return new SearchResults
            {
                Shirts = matchedShirts,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };
        }

        public SearchResults SearchParallel(SearchOptions options)
        {
            if (options == null || options.Colors == null || options.Sizes == null)
            {
                throw new ArgumentNullException("SearchOptions or one of its properties are null");
            }

            var matchedShirts = (from shirt in _shirts.AsParallel()
                                 where (!options.Colors.Any() || options.Colors.Any(clr => clr == shirt.Color))
                                      && (!options.Sizes.Any() || options.Sizes.Any(sze => sze == shirt.Size))
                                 select shirt).ToList();

            var sizeCounts = matchedShirts.GroupBy(shirt => shirt.Size).Select(shirtGrp => new SizeCount() { Size = shirtGrp.Key, Count = shirtGrp.Count() }).ToList();
            sizeCounts.AddRange(Size.All.Where(size => !sizeCounts.Any(sze => sze.Size == size)).Select(sze => new SizeCount { Size = sze, Count = 0 }).ToList());

            var colorCounts = matchedShirts.GroupBy(shirt => shirt.Color).Select(colorGrp => new ColorCount() { Color = colorGrp.Key, Count = colorGrp.Count() }).ToList();
            colorCounts.AddRange(Color.All.Where(color => !colorCounts.Any(clr => clr.Color == color)).Select(clr => new ColorCount { Color = clr, Count = 0 }).ToList());


            return new SearchResults
            {
                Shirts = matchedShirts,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };
        }

        public SearchResults SearchWithoutLinq(SearchOptions options)
        {
            if (options == null || options.Colors == null || options.Sizes == null)
            {
                throw new ArgumentNullException("SearchOptions or one of its properties are null");
            }

            var anyColors = options.Colors.Any();
            var anySizes = options.Sizes.Any();

            foreach (var shirt in _shirts)
            {
                Color matchedColour = null;
                Size matchedSize = null;

                if (anyColors)
                {
                    matchedColour = options.Colors.SingleOrDefault(color => shirt.Color == color);
                }

                if (anySizes)
                {
                    matchedSize = options.Sizes.SingleOrDefault(size => shirt.Size == size);
                }

                if ((!anySizes || matchedSize != null) && (!anyColors || matchedColour != null))
                {
                    _matches.Add(shirt);
                    _sizeCount[shirt.Size.Id].Count++;
                    _colorCount[shirt.Color.Id].Count++;
                }
            }

            return new SearchResults
            {
                Shirts = _matches,
                ColorCounts = _colorCount.Select(cnt => cnt.Value).ToList(),
                SizeCounts = _sizeCount.Select(sze => sze.Value).ToList()
            };
        }
    }
}