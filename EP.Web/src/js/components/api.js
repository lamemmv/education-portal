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

    getFile(id) {
        let url = `${baseUri}admin/blobManager/File/${id}`;

        return axios({
            method: 'get',
            url: url,
            headers: []
        });
    },

    uploadFiles(request) {
        if (request.parent == null) {
            request.parent = '';
        }

        let body = new FormData();
        request.files.map(file => {
            body.append("files", file);
        });
        body.append("parent", request.parent);

        let headers = new Headers({
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'multipart/form-data'
        });

        return axios({
            method: 'post',
            data: body,
            url: `${baseUri}admin/blobManager/File`,
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

    getNewsById(id) {
        let url = `${baseUri}admin/newsManager/${id}`;

        return axios({
            method: 'get',
            url: url,
            headers: []
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

    updateNews(request) {
        let url = `${baseUri}admin/newsManager/${request.id}`;
        return axios({
            method: 'put',
            url: url,
            data: {
                title: request.title,
                content: request.content,
                ingress: request.ingress,
                published: request.published,
                blobId: request.blobId
            },
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },

    deleteNews(id) {
        let url = `${baseUri}admin/newsManager/${id}`;
        return axios({
            method: 'delete',
            url: url,
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

    deleteFolder(id) {
        let body = new FormData();
        body.append("id", id);
        let url = `${baseUri}admin/BlobManager`;
        return axios({
            method: 'delete',
            url: url,
            data: body,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },

    deleteFolders(nodes) {
        let body = new FormData();
        nodes.map((node) => {
            if (node.selected) {
                body.append("ids", node.id);
            }
        });

        let url = `${baseUri}admin/BlobManager`;
        return axios({
            method: 'delete',
            url: url,
            data: body,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
    },
}

export default API;