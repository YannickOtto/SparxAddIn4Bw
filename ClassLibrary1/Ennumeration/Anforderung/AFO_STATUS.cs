///////////////////////////////////////////////////////////
//  AFO_STATUS.cs
//  Implementation of the Enumeration AFO_STATUS
//  Generated by Enterprise Architect
//  Created on:      01-Aug-2018 15:35:43
//  Original author: OLtzS Yannick Otto
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace Ennumerationen
{
	public enum AFO_STATUS : int {

		neu,
		inAbstimmung,
		befuerwortet,
		zurueckgestellt,
		festgeschrieben,
		realisiert,
		obsolet,
        NULL
        //{"neu", "in Abstimmung", "befuerwortet", "zurueckgestellt", "festgeschrieben", "realisiert", "obsolet"};
    }//end AFO_STATUS

}//end namespace Anforderung