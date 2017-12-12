import axios from 'axios';
import API from '../api';
import PageSetting from '../../settings/page';

//Post list
export const GET_FILES = 'GET_FILES';
export const GET_FILES_SUCCESS = 'GET_FILES_SUCCESS';
export const GET_FILES_FAILURE = 'GET_FILES_FAILURE';

const ROOT_URL = API.getBaseUri();

export function getFiles(currentPage) {
    let params = { page: currentPage ? currentPage : 1, size: PageSetting.getPageSize() };
    let url = `${ROOT_URL}admin/blobManager`;
    url += (url.indexOf('?') === -1 ? '?' : '&') + API.queryParams(params);
    const request = axios({
        method: 'get',
        url: url,
        headers: []
    });

    return {
        type: GET_FILES,
        payload: request
    };
}

export function getFilesSuccess(files) {
    return {
        type: GET_FILES_SUCCESS,
        payload: files
    };
}

export function getFilesFailure(error) {
    return {
        type: GET_FILES_FAILURE,
        payload: error
    };
}