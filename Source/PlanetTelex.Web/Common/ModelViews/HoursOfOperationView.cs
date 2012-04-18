/**
 * Copyright (c) 2012 Planet Telex Inc. all rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
﻿using System;
using System.Text;
using PlanetTelex.Common.Models;
using PlanetTelex.Web.Properties;

namespace PlanetTelex.Web.Common.ModelViews
{
    /// <summary>
    /// A view of an HoursOfOperation class.
    /// </summary>
    public class HoursOfOperationView : IModelView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HoursOfOperationView"/> class.
        /// </summary>
        /// <param name="hoursOfOperation">The hours of operation.</param>
        public HoursOfOperationView(HoursOfOperation hoursOfOperation)
        {
            _hoursOfOperation = hoursOfOperation;
        }
        private readonly HoursOfOperation _hoursOfOperation;
        
        /// <summary>
        /// Creates an HTML representation of this instance.
        /// </summary>
        public string Html()
        {
            bool isSame = false;
            BusinessHours day;
            BusinessHours nextday;

            StringBuilder hoursHtml = new StringBuilder(HtmlStrings.STRONG);
            hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Monday));

            for (int i = 1; i < 6; i++)
            {
                day = _hoursOfOperation.DailyHours[i];
                nextday = _hoursOfOperation.DailyHours[i + 1];

                if (day.IsClosed && nextday.IsClosed)
                    isSame = true;
                else if (day.IsClosed && !nextday.IsClosed)
                {
                    if (isSame)
                        hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), i - 1));

                    hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                    hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlClosed + " ");
                    hoursHtml.Append(HtmlStrings.BR + HtmlStrings.STRONG);
                    hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), i + 1));
                    isSame = false;
                }
                else if (String.CompareOrdinal(day.DisplayOpenTime, nextday.DisplayOpenTime) != 0 || String.CompareOrdinal(day.DisplayCloseTime, nextday.DisplayCloseTime) != 0)
                {
                    if (isSame)
                        hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), i - 1));

                    hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                    hoursHtml.Append(day.DisplayOpenTime);
                    hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                    hoursHtml.Append(day.DisplayCloseTime);
                    hoursHtml.Append(HtmlStrings.BR + HtmlStrings.STRONG);
                    hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), i + 1));
                    isSame = false;
                }
                else
                    isSame = true;
            }

            // Special case for Sunday in order to include it at the end
            day = _hoursOfOperation.DailyHours[6];
            nextday = _hoursOfOperation.DailyHours[0];

            if (day.IsClosed && nextday.IsClosed)
            {
                hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), 0));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlClosed + " ");
            }
            else if (day.IsClosed && !nextday.IsClosed)
            {
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlClosed + " ");
                hoursHtml.Append(HtmlStrings.BR + HtmlStrings.STRONG);
                hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), 0));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(nextday.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(nextday.DisplayCloseTime + HtmlStrings.BR);
            }
            else if (String.CompareOrdinal(day.DisplayOpenTime, nextday.DisplayOpenTime) != 0 || String.CompareOrdinal(day.DisplayCloseTime, nextday.DisplayCloseTime) != 0)
            {
                if (isSame)
                    hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), 6));

                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(day.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(day.DisplayCloseTime);
                hoursHtml.Append(HtmlStrings.BR + HtmlStrings.STRONG);
                hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), 0));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(nextday.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(nextday.DisplayCloseTime + HtmlStrings.BR);
            }
            else if (String.CompareOrdinal(day.DisplayOpenTime, nextday.DisplayOpenTime) == 0 && String.CompareOrdinal(day.DisplayCloseTime, nextday.DisplayCloseTime) == 0 && isSame)
            {
                hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), 0));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(day.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(day.DisplayCloseTime);
            }
            else if (String.CompareOrdinal(day.DisplayOpenTime, nextday.DisplayOpenTime) == 0 && String.CompareOrdinal(day.DisplayCloseTime, nextday.DisplayCloseTime) == 0 && !isSame)
            {
                hoursHtml.Append(" - " + Enum.GetName(typeof(DayOfWeek), 6));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(day.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(day.DisplayCloseTime);
            }
            else
            {
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(day.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(day.DisplayCloseTime);
                hoursHtml.Append(HtmlStrings.BR + HtmlStrings.STRONG);
                hoursHtml.Append(Enum.GetName(typeof(DayOfWeek), 0));
                hoursHtml.Append(HtmlStrings.STRONG_END + HtmlStrings.BR + HtmlStrings.NBSP + HtmlStrings.NBSP);
                hoursHtml.Append(nextday.DisplayOpenTime);
                hoursHtml.Append(" " + Resources.GenerateBusinessHoursHtmlTo + " ");
                hoursHtml.Append(nextday.DisplayCloseTime + HtmlStrings.BR);
            }

            return hoursHtml.ToString().Replace("12:00 AM " + Resources.GenerateBusinessHoursHtmlTo + " 12:00 AM", Resources.GenerateBuisinessHoursHtmlOpen24Hours);
        }
    }
}
