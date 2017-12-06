// A simple data API that will be used to get the data for our
// components. On a real website, a more robust data fetching
// solution would be more appropriate.
const baseUri = 'http://localhost:52861/api/';
const API = {    
    getNews(userData) {
        fetch(baseUri  + 'admin/news', {
            method: 'get',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                if (response.status >= 200 && response.status < 300) {
                    console.log(response);
                } else {
                    const error = new Error(response.statusText);
                    error.response = response;
                    throw error;
                }
            })
            .catch(error => { console.log('request failed', error); });
    }
}

export default API;
