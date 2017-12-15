import axios from 'axios';
import PageSetting from '../settings/page';

const baseUri = 'http://localhost:52860/api/';

const API = {
    getBaseUri() {
        return baseUri;
    },

    queryParams(params: any) {
        return Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
    },

    getNews(page: number) {
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
    },

    getFiles(page: number) {
        let params = { page: page ? page : 1, size: PageSetting.getPageSize() };
        let url = `${baseUri}admin/blobManager`;
        url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(params);

        return axios({
            method: 'get',
            url: url,
            headers: []
        });
    }
}

export default API;
