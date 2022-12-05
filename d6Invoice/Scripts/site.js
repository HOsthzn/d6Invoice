const app =
{
    request(headers, path, method, queryString, payload ) {
        return new Promise( (resolved, rejected) => {
            //defaults
            headers = typeof headers === "object" && headers !== null
                      ? headers
                      : { };
            path = typeof path === "string"
                   ? path
                   : "/";
            method = typeof method === "string"
                     && [ "GET", "POST", "PUT", "DELETE" ].indexOf( method.toUpperCase( ) )
                     > -1
                     ? method.toUpperCase( )
                     : "GET";
            queryString = typeof queryString === "object" && queryString !== null
                          ? queryString
                          : { };
            payload = typeof payload === "object" && payload !== null
                      ? payload
                      : { };

            let requestUrl = path + "?";
            let count = 0;
            for( let key in queryString ) {
                if( queryString.hasOwnProperty( key ) ) {
                    count++;
                    if( count > 1 ) requestUrl += "&";
                    requestUrl += `${ key }=${ queryString[ key ] }`;
                }
            }

            const xhr = new XMLHttpRequest( );
            xhr.open( method, requestUrl, true );
            xhr.setRequestHeader( "Content-type", "application/json" );

            for( let key in headers ) if( headers.hasOwnProperty( key ) ) xhr.setRequestHeader( key, headers[ key ] );

            xhr.onreadystatechange = function( ) {
                try {
                    if( xhr.readyState === XMLHttpRequest.DONE ) {
                        const statusCode = xhr.status;
                        const response = xhr.responseText;

                        const result = JSON.parse( response );
                        resolved( { statusCode, result } );
                    }
                } catch( e ) {
                    rejected( e );
                }
            };

            xhr.send( JSON.stringify( payload ) );
        } );
    }
    , localStorage: {
        get: function( key ) {
            //get value from local storage
            key = this.key( key );
            return JSON.parse( localStorage.getItem( key ) );
        }
        , set: function( key, value ) {
            //set value in local storage
            key = this.key( key );
            localStorage.setItem( key, JSON.stringify( value ) );
            return this.get( key );
        }
        , remove: function( key ) {
            //remove value from local storage
            key = this.key( key );
            localStorage.removeItem( key );
        }
        , clear: function( ) {
            //clear the local storage
            localStorage.clear( );
        }
        , Exists: function( key ) {
            //check if value exists in local storage
            key = this.key( key );
            return this.get( key ) !== null;
        }
        , Event: function( callBack ) {
            //add storage change event on local storage
            window.addEventListener( "storage", callBack );
        }
        , key: function( key ) {
            //transform value to valid key
            key = typeof key === "string" && key.trim( ).length > 0
                  ? key
                  : false;
            if( key && key !== "#" ) return key.toLowerCase( ).replace( /\s/g, "" );
            else throw new Error( "invalid key value" );
        }
    }
};

const clients = {
    pageNumber: 0
    , pageCount: 0
    , init() {
        const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
        ddlRecPerPage.addEventListener( "change", clients.recordsPerPage );

        clients.GetClients( clients.pageNumber, parseInt( ddlRecPerPage.value ) );

        document.getElementById( "btnPrevious" ).addEventListener( "click", clients.pagination.previous );
        document.getElementById( "btnNext" ).addEventListener( "click", clients.pagination.next );
        document.querySelector( ".pagination" ).addEventListener( "click", clients.pagination.selectPage );
    }
    , GetClients(page, recPerPage) {
        app.request( undefined, "/Client/GetClients", "GET", { page, recPerPage }, undefined )
            .then( ({ statusCode, result }) => {
                if( statusCode === 200 ) {
                    const tbClients = document.getElementById( "tbClients" );
                    //clear tBody
                    tbClients.tBodies[ 0 ].innerHTML = "";
                    let altRowCount = 0;
                    for( const client of result.Clients ) {
                        //add row to table
                        const row = tbClients.tBodies[ 0 ].insertRow( );

                        //add class to alternating rows
                        altRowCount++;
                        if( ( altRowCount % 2 ) === 0 ) row.classList.add( "altRow" );

                        //add cells to row
                        for( const key in client )
                            if( client.hasOwnProperty( key ) )
                                if( key !== "Id" ) row.insertCell( ).innerText = client[ key ];

                        // record btn's
                        const btnEdit = btn( "Edit", `/Client/Edit/${ client.Id }`, "btn", "btn-primary" );
                        const btnDetails = btn( "Details", `/Client/Details/${ client.Id }`, "btn", "btn-info" );
                        const btnDelete = btn( "Delete", `/Client/Delete/${ client.Id }`, "btn", "btn-danger" );

                        row.insertCell( ).innerHTML =
                            `${ btnEdit.outerHTML } | ${ btnDetails.outerHTML } | ${ btnDelete.outerHTML }`;
                    }

                    clients.pagination.PageCount( result.PageCount );

                    function btn( text, href, ...classItems ) {
                        const result = document.createElement( "a" );
                        classItems.forEach( c => result.classList.add( c ) );
                        result.href = href;
                        result.innerText = text;
                        return result;
                    }
                }
            } )
            .catch( err => console.log( err ) );
    }
    , pagination: {
        next() {
            if( clients.pageNumber < clients.pageCount - 1) {
                const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
                clients.pageNumber++;
                clients.GetClients( clients.pageNumber, parseInt( ddlRecPerPage.value ) );
            }
        }
        , previous() {
            if( clients.pageNumber > 0 ) {
                const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
                clients.pageNumber--;
                clients.GetClients( clients.pageNumber, parseInt( ddlRecPerPage.value ) );
            }
        }
        , selectPage(e) {
            const parentElement = e.target.parentElement;
            const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );

            //could also have used indexOf >= 1 (older browsers doesn't support includes)
            if( [ "btnPrevious", "btnNext" ].includes( parentElement.id ) ) return;
            clients.pageNumber = parentElement.value;
            clients.GetClients( clients.pageNumber , parseInt( ddlRecPerPage.value ) );
        }
        , PageCount(numOfPages) {
            const btnNext = document.getElementById( "btnNext" );

            clients.pageCount = numOfPages;

            document.querySelectorAll( ".pageNum" ).forEach( pn => pn.remove( ) );
            for( let i = 1; i <= numOfPages; i++ ) {

                const a = document.createElement( "a" );
                a.classList.add( "page-link" );
                a.href = "#";
                a.innerText = i;

                const li = document.createElement( "li" );
                li.classList.add( "page-item", "pageNum" );
                li.appendChild( a );
                li.value = i - 1;

                if( li.value === clients.pageNumber ) li.classList.add( "active" );

                btnNext.insertAdjacentHTML( "beforebegin", li.outerHTML );
            }
        }
    }
    , recordsPerPage(e) { clients.GetClients( clients.pageNumber, parseInt( e.target.value ) ); }
};

const products = {
      pageNumber: 0
    , pageCount: 0
    , init() {
        const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
        ddlRecPerPage.addEventListener( "change", products.recordsPerPage );

        products.GetProducts( products.pageNumber, parseInt( ddlRecPerPage.value ) );

        document.getElementById( "btnPrevious" ).addEventListener( "click", products.pagination.previous );
        document.getElementById( "btnNext" ).addEventListener( "click", products.pagination.next );
        document.querySelector( ".pagination" ).addEventListener( "click", products.pagination.selectPage );
    }
    , GetProducts(page, recPerPage) {
        app.request( undefined, "/Product/GetProducts", "GET", { page, recPerPage }, undefined )
            .then( ({ statusCode, result }) => {
                if( statusCode === 200 ) {
                    const tbProducts = document.getElementById( "tbProducts" );
                    //clear tBody
                    tbProducts.tBodies[ 0 ].innerHTML = "";
                    let altRowCount = 0;
                    for( const product of result.Products ) {
                        //add row to table
                        const row = tbProducts.tBodies[ 0 ].insertRow( );

                        //add class to alternating rows
                        altRowCount++;
                        if( ( altRowCount % 2 ) === 0 ) row.classList.add( "altRow" );

                        //add cells to row
                        for( const key in product )
                            if( product.hasOwnProperty( key ) )
                                if( key !== "Id" ) row.insertCell( ).innerText = product[ key ];

                        // record btn's
                        const btnEdit = btn( "Edit", `/Product/Edit/${ product.Id }`, "btn", "btn-primary" );
                        const btnDetails = btn( "Details", `/Product/Details/${ product.Id }`, "btn", "btn-info" );
                        const btnDelete = btn( "Delete", `/Product/Delete/${ product.Id }`, "btn", "btn-danger" );

                        row.insertCell( ).innerHTML =
                            `${ btnEdit.outerHTML } | ${ btnDetails.outerHTML } | ${ btnDelete.outerHTML }`;
                    }

                    products.pagination.PageCount( result.PageCount );

                    function btn( text, href, ...classItems ) {
                        const result = document.createElement( "a" );
                        classItems.forEach( c => result.classList.add( c ) );
                        result.href = href;
                        result.innerText = text;

                        return result;
                    }

                }
            } )
            .catch( err => console.log( err ) );
    }
    , pagination: {
        next() {
            if( products.pageNumber < products.pageCount - 1) {
                const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
                products.pageNumber++;
                products.GetProducts( products.pageNumber, parseInt( ddlRecPerPage.value ) );
            }
        }
        , previous() {
            if( products.pageNumber > 0 ) {
                const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
                products.pageNumber--;
                products.GetProducts( pageNumber.pageNumber, parseInt( ddlRecPerPage.value ) );
            }
        }
        , selectPage(e) {
            const parentElement = e.target.parentElement;
            const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );

            //could also have used indexOf >= 1 (older browsers doesn't support includes)
            if( [ "btnPrevious", "btnNext" ].includes( parentElement.id ) ) return;
            products.pageNumber = parentElement.value;
            products.GetProducts( products.pageNumber , parseInt( ddlRecPerPage.value ) );

            
        }
        , PageCount(numOfPages) {
            const btnNext = document.getElementById( "btnNext" );

            products.pageCount = numOfPages;

            document.querySelectorAll( ".pageNum" ).forEach( pn => pn.remove( ) );
            for( let i = 1; i <= numOfPages; i++ ) {

                const a = document.createElement( "a" );
                a.classList.add( "page-link" );
                a.href = "#";
                a.innerText = i;

                const li = document.createElement( "li" );
                li.classList.add( "page-item", "pageNum" );
                li.appendChild( a );
                li.value = i - 1;

                if( li.value === products.pageNumber ) li.classList.add( "active" );

                btnNext.insertAdjacentHTML( "beforebegin", li.outerHTML );
            }
        }
    }
    , recordsPerPage(e) { products.GetProducts( products.pageNumber, parseInt( e.target.value ) ); }
};

const invoices = { };
const invoicesHeaders = { };
const invoicesDetails = { };
