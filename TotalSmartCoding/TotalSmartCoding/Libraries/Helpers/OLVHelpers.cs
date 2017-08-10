using System;
using System.Collections.Generic;

using BrightIdeasSoftware;

namespace TotalSmartCoding.Libraries.Helpers
{
    public static class OLVHelpers
    {
        public static void ApplyFilters(ObjectListView olv, string[] filterTexts)
        {
            OLVHelpers.ApplyFilters(olv, filterTexts, 0);
        }

        public static void ApplyFilters(ObjectListView olv, string[] filterTexts, int matchKind)
        {
            TextMatchFilter textMatchFilter = null;//Using this textMatchFilter for hightlight purpose
            List<IModelFilter> modelFilters = null;

            if (filterTexts != null && filterTexts.Length > 0)
            {
                textMatchFilter = OLVHelpers.createTextMatchFilter(olv, filterTexts, matchKind);

                modelFilters = new List<IModelFilter>();
                foreach (string s in filterTexts)
                {
                    modelFilters.Add(OLVHelpers.createTextMatchFilter(olv, new string[] { s }, matchKind));
                }

            }

            // Setup a default renderer to draw the filter matches
            if (textMatchFilter == null)
                olv.DefaultRenderer = null;
            else
            {
                olv.DefaultRenderer = new HighlightTextRenderer(textMatchFilter);

                // Uncomment this line to see how the GDI+ rendering looks
                //olv.DefaultRenderer = new HighlightTextRenderer { Filter = filter, UseGdiTextRendering = false };
            }

            // Some lists have renderers already installed
            HighlightTextRenderer highlightingRenderer = olv.GetColumn(0).Renderer as HighlightTextRenderer;
            if (highlightingRenderer != null)
                highlightingRenderer.Filter = textMatchFilter;


            olv.ModelFilter = modelFilters == null ? null : new CompositeAllFilter(modelFilters); //WHEN USING CompositeAllFilter, this textMatchFilter for HIGHTLIGHT PURPOSE ONLY
            //olv.AdditionalFilter = textMatchFilter; //WHEN USING AdditionalFilter: In addition to use this textMatchFilter for hightlight purpose, this textMatchFilter CAN BE USE FOR AdditionalFilter TO FILLER
            //NOTE:     AdditionalFilter: TO FILTER rows where any cell match any of the given strings array (string[]) --- MEANING: USING 'OR' CLAUSE
            //          ModelFilter with CompositeAllFilter: CREATE SEPARATE textMatchFilter FOR EVERY item IN strings array (string[]), THEN COMBINE ALL FILTER TOGETHER --- MEANING: USING 'AND' CLAUSE
            //ONLY USING AdditionalFilter OR CompositeAllFilter AT ONE TIME

            //olv.Invalidate();
        }




        private static TextMatchFilter createTextMatchFilter(ObjectListView olv, string[] filterTexts, int matchKind)
        {
            switch (matchKind)
            {
                case 0:
                default:
                    return TextMatchFilter.Contains(olv, filterTexts);
                case 1:
                    return TextMatchFilter.Prefix(olv, filterTexts);
                case 2:
                    return TextMatchFilter.Regex(olv, filterTexts);
            }
        }


    }
}
