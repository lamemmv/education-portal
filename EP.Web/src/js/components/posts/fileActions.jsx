import axios from 'axios';

//Post list
export const GET_FILES = 'GET_FILES';
export const GET_FILES_SUCCESS = 'GET_FILES_SUCCESS';
export const GET_FILES_FAILURE = 'GET_FILES_FAILURE';

export function getFiles(page) {
    return {
        type: GET_FILES,
        payload: page
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