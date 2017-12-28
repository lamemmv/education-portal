import {
    DELETE_FILE,
    DELETE_FILE_FAILURE,
    DELETE_FILE_SUCCESS,
    ASK_FOR_DELETING_FILE,
    SHOW_CONFOIRMATION_DIALOG,
    CLOSE_MODAL_CONFIRMATION
} from '../types';

const INITIAL_STATE = {
    deleteState: {
        showDeleteConfirmation: false,
        fileTobeDeleted: null,
        error: null,
        loading: false
    }
};

export default function (state = INITIAL_STATE, action) {
    let error;
    switch (action.type) {
        case DELETE_FILE:
            return { ...state,
                deleteState: {
                    showDeleteConfirmation: true,
                    loading: true
                }
            };
        case DELETE_FILE_SUCCESS:
            return {
                ...state,
                deleteState: {
                    loading: false,
                    error: null,
                    fileTobeDeleted: null,
                    showDeleteConfirmation: false
                }
            };
        case DELETE_FILE_FAILURE:
            error = action.payload || {
                message: action.payload.message
            };
            return {
                ...state,
                deleteState: {
                    fileTobeDeleted: null,
                    showDeleteConfirmation: false,
                    error: error,
                    loading: false
                }
            };
        case ASK_FOR_DELETING_FILE:
            return {
                ...state,
                deleteState: {
                    fileTobeDeleted: action.payload,
                    showDeleteConfirmation: false,
                    loading: false,
                    error: null
                }
            };
        case SHOW_CONFOIRMATION_DIALOG:
            return {
                ...state,
                deleteState: {
                    fileTobeDeleted: action.payload,
                    showDeleteConfirmation: true,
                    loading: false,
                    error: null
                }
            };
        case CLOSE_MODAL_CONFIRMATION:
            return {
                ...state,
                deleteState: {
                    fileTobeDeleted: null,
                    showDeleteConfirmation: action.payload,
                    loading: false,
                    error: null
                }
            };
        default:
            return state;
    }
}