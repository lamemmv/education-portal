import {
    GET_FILES,
    GET_FILES_SUCCESS,
    GET_FILES_FAILURE
} from './types';

export function getFiles(filter) {
    return {
        type: GET_FILES,
        payload: filter
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