import axios from 'axios';
import PageSetting from '../settings/page';

const baseUri = 'http://localhost:52860/api/';

const API = {
    getBaseUri() {
        return baseUri;
    },

    queryParams(params) {
        return Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
    },

    getNews(page) {
        let params = { page: page ? page : 1, size: PageSetting.getPageSize() };
        let url = `${baseUri}admin/newsManager`;
        url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(params);
        let headers = new Headers({
            'Access-Control-Allow-Origin': '*'
        });

        return axios({
            method: 'get',
            url: url,
            headers: headers
        });
    }
}

export default API;
