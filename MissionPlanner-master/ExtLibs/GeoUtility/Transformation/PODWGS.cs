//===================================================================================================
// Source Control URL : $HeadURL: file:///D:/svn/branch/3.1.7.0/GeoUtility/Transformation/PODWGS.cs $
// Last changed by    : $LastChangedBy: sh $
// Revision           : $LastChangedRevision: 255 $
// Last changed date  : $LastChangedDate: 2011-04-30 11:15:00 +0200 (Sat, 30. Apr 2011) $
// Author             : $Author: sh $
// Copyright	      : Copyight (c) 2009-2011 Steffen Habermehl
// Contact            : geoutility@freenet.de
// License            : GNU GENERAL PUBLIC LICENSE Ver. 3, GPLv3
//                      This program is free software; you can redistribute it and/or modify it under the terms 
//                      of the GNU General Public License as published by the Free Software Foundation; 
//                      either version 3 of the License, or (at your option) any later version. 
//                      This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
//                      without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//                      See the GNU General Public License for more details. You should have received a copy of the 
//                      GNU General Public License along with this program; if not, see <http://www.gnu.org/licenses/>. 
//
// File description   : Klasse Transformation, Funktion: PODWGS 
//===================================================================================================



using System;
using GeoUtility.GeoSystem;
using GeoUtility.GeoSystem.Helper;


namespace GeoUtility
{
    internal partial class Transformation
    {

        /// <summary><para>Die Funktion verschiebt das Datum von <see cref="GeoUtility.GeoSystem.Helper.GeoDatum.Potsdam">Potsdam</see> (nur Deutschland) nach <see cref="GeoUtility.GeoSystem.Helper.GeoDatum.WGS84">WGS84</see> (international).
        /// <para>Die Funktion ist nur für interne Berechnungen bestimmt.</para></para></summary>
        /// <remarks><para>
        /// Hintergründe zum Problem der Koordinatentransformationen sowie entsprechende  mathematische 
        /// Formeln können den einschlägigen Fachbüchern oder dem Internet entnommen werden.<p />
        /// Quellen: 
        /// Bundesamt für Kartographie und Geodäsie<br />
        /// <a href="http://www.bkg.bund.de" target="_blank">http://www.bkg.bund.de</a><br />
        /// <a href="http://crs.bkg.bund.de" target="_blank">http://crs.bkg.bund.de</a><br />
        /// </para></remarks>
        /// 
        /// <param name="geo">Ein <see cref="GeoUtility.GeoSystem.Geographic"/>-Objekt im <see cref="GeoUtility.GeoSystem.Helper.GeoDatum.Potsdam">Potsdam-Datum</see></param>
        /// <returns>Ein <see cref="GeoUtility.GeoSystem.Geographic"/>-Objekt im <see cref="GeoUtility.GeoSystem.Helper.GeoDatum.WGS84">WGS84-Datum</see>.</returns>
        internal Geographic PODWGS(Geographic geo)
        {

            double laengePotsdam = geo.Longitude;
            double breitePotsdam = geo.Latitude;

            // Breite und Länge (Rad)
            double breiteRad = breitePotsdam * (Math.PI / 180);
            double laengeRad = laengePotsdam * (Math.PI / 180);

            // Querkrümmung
            double qkhm = HALBACHSE / Math.Sqrt(1 - EXZENT * Math.Sin(breiteRad) * Math.Sin(breiteRad));

            // Kartesische Koordinaten Potsdam
            double xPotsdam = qkhm * Math.Cos(breiteRad) * Math.Cos(laengeRad);
            double yPotsdam = qkhm * Math.Cos(breiteRad) * Math.Sin(laengeRad);
            double zPotsdam = (1 - EXZENT) * qkhm * Math.Sin(breiteRad);

            // Kartesische Koordinaten WGS84
            double x = xPotsdam + POTSDAM_DATUM_SHIFT_X;
            double y = yPotsdam + POTSDAM_DATUM_SHIFT_Y;
            double z = zPotsdam + POTSDAM_DATUM_SHIFT_Z;

            // Breite und Länge im WGS84 Datum
            double b = Math.Sqrt(x * x + y * y);
            double breite = (180 / Math.PI) * Math.Atan((z / b) / (1 - WGS84_EXZENT));

            double laenge = 0;
            if (x > 0) laenge = (180 / Math.PI) * Math.Atan(y / x);
            if (x < 0 && y > 0) laenge = (180 / Math.PI) * Math.Atan(y / x) + 180;
            if (x < 0 && y < 0) laenge = (180 / Math.PI) * Math.Atan(y / x) - 180;

            return new Geographic(laenge, breite, GeoDatum.WGS84);
        }

    }

}
