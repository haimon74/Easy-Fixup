/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    [Editable]
    public partial class SearchBoxWebPart : WebPartUserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Quick Search");
        }

    }
}