import {
    DELETE_FILE,
    DELETE_FILE_SUCCESS,
    DELETE_FILE_FAILURE,
    SHOW_CONFOIRMATION_DIALOG,
    CLOSE_MODAL_CONFIRMATION,
    ASK_FOR_DELETING_FILE
} from '../types';

export function deleteFile(id) {
    return {
        type: DELETE_FILE,
        payload: id
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

export function showModal(id) {
    return {
        type: SHOW_CONFOIRMATION_DIALOG,
        payload: id
    };
}

export function closeModal() {
    return {
        type: CLOSE_MODAL_CONFIRMATION,
        payload: false
    };
}