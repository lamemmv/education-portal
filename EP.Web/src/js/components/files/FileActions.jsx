export const GET_FILES = "GET_FILES";
export const UPLOAD_FILE = "UPLOAD_FILE";
export const VIEW_FILE = "VIEW_FILE";
export const DELETE_FILE = "DELETE_FILE";
export const REQUEST_POSTS = 'REQUEST_POSTS';
export const RECEIVE_POSTS = 'RECEIVE_POSTS';
export const INVALIDATE_FILES = 'INVALIDATE_FILES';
import API from '../api';
import PageSetting from '../../settings/page';

export function getFiles(page, size, extension) {
    return {
        type: GET_FILES,
        payload: {
            page: page,
            size: size,
            extension: extension
        }
    }
}

export const invalidateFile = currentPage => ({
    type: INVALIDATE_FILES,
    currentPage
})

export const requestPosts = currentPage => ({
    type: REQUEST_POSTS,
    currentPage
})

function receivePosts(currentPage, json) {
    return {
        type: RECEIVE_POSTS,
        currentPage,
        posts: json.data.children.map(child => child.data),
        receivedAt: Date.now()
    }
}

const fetchPosts = currentPage => dispatch => {
    dispatch(requestPosts(currentPage));
    let request = { page: currentPage ? currentPage : 1, size: PageSetting.getPageSize() };
    let url = API.getBaseUri() + 'admin/blobManager';
    url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(request);
    return fetch(url, {
        method: 'get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }
    })
        .then(response => response.json())
        .then(json => dispatch(receivePosts(currentPage, json)))
}

export function uploadFile(file) {
    return {
        type: UPLOAD_FILE,
        payload: {
            file: file
        }
    }
}

export function previewFile(id) {
    return {
        type: VIEW_FILE,
        payload: id
    }
}

export function deleteFile(id) {
    return {
        type: DELETE_FILE,
        payload: id
    }
}