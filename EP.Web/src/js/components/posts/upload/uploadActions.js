import axios from 'axios';
import API from '../../api';

export const SELECTED_FILE = 'SELECTED_FILE';
export const UPLOAD_FILE = 'UPLOAD_FILE';
export const UPLOAD_FILE_SUCCESS = 'UPLOAD_FILE_SUCCESS';
export const UPLOAD_FILE_FAILURE = 'UPLOAD_FILE_FAILURE';
export const REMOVE_FILE = 'REMOVE_FILE';

const ROOT_URL = API.getBaseUri();

export function selectFile(file) {
    return {
        type: SELECTED_FILE,
        payload: file
    };
}

export function uploadFile(files) {
    let body = new FormData();
    files.map(file => {
        body.append("files", file);
    });

    let headers = new Headers({
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'multipart/form-data'
    });

    const request = axios({
        method: 'post',
        data: body,
        url: `${ROOT_URL}admin/blobManager`,
        headers: headers
    });

    return {
        type: UPLOAD_FILE,
        payload: request
    };
}

export function uploadFileSuccess(id) {
    return {
        type: UPLOAD_FILE_SUCCESS,
        payload: id
    };
}

export function uploadFileFailure(error) {
    return {
        type: UPLOAD_FILE_FAILURE,
        payload: error
    };
}

export function removeFile(file) {
    return {
        type: REMOVE_FILE,
        payload: file
    };
}