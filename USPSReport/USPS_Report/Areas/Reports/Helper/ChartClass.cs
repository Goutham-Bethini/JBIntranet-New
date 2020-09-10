using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using USPS_Report.Areas.Reports.Models;
using System.Drawing;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Attributes;

namespace USPS_Report.Areas.Reports
{
    public class ChartClass
    {
        public static Highcharts ShippingOrderChart(IList<ShippingOrderData> _list)
        {




            var xDataMonths = _list.Select(i => String.Format("{0:MM/dd/yy}",i.Date)).ToArray();

            var yDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.Hours) }).ToArray();

            var zDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.Worked) }).ToArray();

            return new Highcharts("bpChart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    BackgroundColor = new BackColorOrGradient(new Gradient
                    {
                        LinearGradient = new[] { 0, 0, 0, 400 },
                        Stops = new object[,]
                                 {
                                    { 0, Color.Azure },
                                     { 1, Color.LightBlue }
                                 }
                    })

                })
                .SetTitle(new Title { Text = "Shipping Order By Hours" })
                .SetXAxis(new XAxis { Categories = xDataMonths })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Value" } })
                .SetTooltip(new Tooltip { Enabled = true, Formatter = @"function() {return ' ' + this.x + ': <b>' +  this.y + '</b>' ; }" })
                .SetPlotOptions(new PlotOptions
                {

                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels { Enabled = true },
                        EnableMouseTracking = true
                    }
                })
                .SetSeries(new[] { new Series { Name = "Hours", Data = new Data(yDataCounts) },
                                new Series { Name = "WorkOrder", Data = new Data(zDataCounts) }

               });

        }



        public static Highcharts HoldReasonChart(IList<HoldReasonList> _list)
        {


            var xDataMonths = _list.Select(i => i.HoldReason).ToArray();

            var yDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.Count) }).ToArray();



            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = "Holds By Reasons" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Hold Counts",
                    Data = new Data(
                    
                        _list.Select(t=> new object[] { t.HoldReason, Convert.ToDouble(t.Count)}).ToArray()
                      
                     
                    )
                });

            return chart;
        }


        public static Highcharts HoldPayerChart(IList<woHoldTypes_Qty> _list)
        {


            var xDataMonths = _list.Select(i => i.InsType).ToArray();

            var yDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.Count) }).ToArray();



            Highcharts chart = new Highcharts("chart1")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = "Holds By Payer" })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Hold Counts",
                    Data = new Data(

                        _list.Select(t => new object[] { t.InsType, Convert.ToDouble(t.Count) }).ToArray()


                    )
                });

            return chart;
        }

        public static Highcharts TotalAssessmentChart(IList<totalAssessmentData> _list)
        {




            var xDataMonths = _list.Select(i => String.Format("{0:MM/dd/yy}", i.name)).ToArray();

            var yDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.Total) }).ToArray();

            var zDataCounts = _list.Select(i => new object[] { Convert.ToDouble(i.count) }).ToArray();

            return new Highcharts("assmntChart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    BackgroundColor = new BackColorOrGradient(new Gradient
                    {
                        LinearGradient = new[] { 0, 0, 0, 400 },
                        Stops = new object[,]
                                 {
                                    { 0, Color.Azure },
                                     { 1, Color.LightBlue }
                                 }
                    })

                })
                .SetTitle(new Title { Text = "Total Assessment" })
                .SetXAxis(new XAxis { Categories = xDataMonths })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Value" } })
                .SetTooltip(new Tooltip { Enabled = true, Formatter = @"function() {return ' ' + this.x + ': <b>' +  this.y + '</b>' ; }" })
                .SetPlotOptions(new PlotOptions
                {

                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels { Enabled = true },
                        EnableMouseTracking = true
                    }
                })
                .SetSeries(new[] { new Series { Name = "TotalHrs", Data = new Data(yDataCounts) },
                               // new Series { Name = "TotalNoDays", Data = new Data(zDataCounts) }

               });

        }

    }
}