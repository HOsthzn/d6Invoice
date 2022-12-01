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
}

const clients = {
    init() {
        const ddlRecPerPage = document.getElementById( "ddlRecPerPage" );
        clients.GetClients( 0, parseInt( ddlRecPerPage.value ) );
        ddlRecPerPage.addEventListener( "change", clients.recordsPerPage );
    }
    , GetClients(page, recPerPage) {
        app.request( undefined, "/Client/GetClients", "GET", { page, recPerPage }, undefined )
            .then( ({ statusCode, result }) => {
                if( statusCode === 200 ) {

                    const tbClients = document.getElementById( "tbClients" );
                    //clear tBody
                    tbClients.tBodies[ 0 ].innerHTML = "";
                    let altRowCount = 0;
                    for( const client of result.clients ) {
                        //add row to table
                        const row = tbClients.tBodies[ 0 ].insertRow( );

                        //add class to alternating rows
                        altRowCount++;
                        if( ( altRowCount % 2 ) === 0 ) row.classList.add( "altRow" );

                        //add cells to row
                        for( const key in client )
                            if( client.hasOwnProperty( key ) )
                                if( key !== "Id" ) row.insertCell( ).innerText = client[ key ];
                    }

                    for( let i = 1; i < result.PageCount; i++ ) 
                        console.log( i );


                }
            } )
            .catch( err => console.log( err ) );
    }
    , pagination: {
        next() { }
        , previous() { }
    }
    , recordsPerPage(e) { clients.GetClients( 0, parseInt( e.target.value ) ); }
}

const Products = { }
const Invoices = { }
const InvoicesHeaders = { }
const InvoicesDetails = { }
