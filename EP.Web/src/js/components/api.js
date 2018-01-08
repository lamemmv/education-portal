import axios from 'axios';
import PageSetting from '../settings/page';

const baseUri = 'http://localhost:5000/api/';
const serverDomain = 'http://localhost:5000/';

const API = {
    getBaseUri() {
        return baseUri;
    },

    getServerDomain() {
        return serverDomain;
    },

    queryParams(params) {
        return Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
    },

    getNews(page, size) {
        let params = {
            page: page ? page : 1,
            size: size ? size : PageSetting.getPageSize()
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

    getFiles(filter) {
        let params = {
            page: filter.page ? filter.page : 1,
            size: filter.size ? filter.size : PageSetting.getPageSize()
        };
        if (filter.folderId) {
            params = Object.assign({}, params, {
                id: filter.folderId
            })
        }

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
    },

    createNews(request) {
        let url = `${baseUri}admin/newsManager`;
        return axios({
            method: 'post',
            url: url,
            data: request,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },

    getFolders(page, size) {
        let params = {
            page: page ? page : 1,
            size: size ? size : PageSetting.getPageSize()
        };
        let url = `${baseUri}admin/folders`;
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

    createFolder(request) {
        let url = `${baseUri}admin/BlobManager/Folder`;
        return axios({
            method: 'post',
            url: url,
            data: request,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },

    updateFolder(request) {
        let url = `${baseUri}admin/BlobManager/Folder/${request.id}`;
        return axios({
            method: 'put',
            url: url,
            data: {
                parent: request.parent,
                name: request.name
            },
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },
}

export default API;