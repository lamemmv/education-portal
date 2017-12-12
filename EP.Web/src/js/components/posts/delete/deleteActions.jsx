import axios from 'axios';
import API from '../../api';

export const DELETE_FILE = 'DELETE_FILE';
export const DELETE_FILE_SUCCESS = 'DELETE_FILE_SUCCESS';
export const DELETE_FILE_FAILURE = 'DELETE_FILE_FAILURE';
export const ASK_FOR_DELETING_FILE = 'ASK_FOR_DELETING_FILE';
export const CLOSE_MODAL_CONFIRMATION = 'CLOSE_MODAL_CONFIRMATION';

const ROOT_URL = API.getBaseUri();

export function deleteFile(id) {
    let url = `${ROOT_URL}admin/blobManager?id=`+ id;
    const request = axios({
        method: 'delete',
        url: url,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }
    });

    return {
        type: DELETE_FILE,
        payload: request
    };
}

export function deleteFileSuccess(response) {
    return {
        type: DELETE_FILE_SUCCESS,
        payload: response
    };
}

export function deleteFileFailure(error) {
    return {
        type: DELETE_FILE_FAILURE,
        payload: error
    };
}

export function askForDeleting(id) {
    return {
        type: ASK_FOR_DELETING_FILE,
        payload: id
    };
}

export function closeModal(){
    return {
        type: CLOSE_MODAL_CONFIRMATION,
        payload: false
    };
}