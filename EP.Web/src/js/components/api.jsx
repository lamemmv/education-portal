// A simple data API that will be used to get the data for our
// components. On a real website, a more robust data fetching
// solution would be more appropriate.
const baseUri = 'http://localhost:52860/api/';
const API = {
    getBaseUri() {
        return baseUri;
    },

    upload(file) {
        let body = new FormData();
        body.append("file", file);
        let header = new Headers({
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'multipart/form-data'
        });
        fetch(baseUri + 'admin/blobManager', {
            method: 'post',
            header: header,
            body: body
        })
            .then(response =>  
                {
                if (response.status >= 200 && response.status < 300) {
                    response.json().then((json)=>{
                        console.log(json);
                    });
                } else {
                    const error = new Error(response.statusText);
                    error.response = response;
                    throw error;
                }
            }
        )
            .then((json)=>{
                console.log(json);
            })
            .catch(error => { console.log('request failed', error); });
    }
}

export default API;
