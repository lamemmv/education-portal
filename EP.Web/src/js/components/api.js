import axios from 'axios';
import PageSetting from '../settings/page';

const baseUri = 'http://localhost:5000/api/';

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
        let params = {
            page: page ? page : 1,
            size: PageSetting.getPageSize()
        };
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

    getFiles(page) {
        let params = {
            page: page ? page : 1,
            size: PageSetting.getPageSize()
        };
        let url = `${baseUri}admin/blobManager`;
        url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(params);

        return axios({
            method: 'get',
            url: url,
            headers: []
        });
    },

    uploadFiles(files) {
        let body = new FormData();
        files.map(file => {
            body.append("files", file);
        });

        let headers = new Headers({
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'multipart/form-data'
        });

        return axios({
            method: 'post',
            data: body,
            url: `${baseUri}admin/blobManager`,
            headers: headers
        });
    },

    deleteFile(id) {
        let url = `${baseUri}admin/blobManager?id=` + id;
        return axios({
            method: 'delete',
            url: url,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    }
}

export default API;