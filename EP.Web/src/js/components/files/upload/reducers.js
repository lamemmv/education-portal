import {
    UPLOAD_FILE,
    UPLOAD_FILE_FAILURE,
    UPLOAD_FILE_SUCCESS,
    SELECTED_FILE,
    REMOVE_FILE
} from '../types';

const INITIAL_STATE = {
    uploadState: {
        files: [],
        error: null,
        loading: false
    }
};

export default function (state = INITIAL_STATE, action) {
    let error;
    switch (action.type) {
        case SELECTED_FILE:
            let files = state.uploadState.files.slice();
            if (action.payload.single) {
                if (files.length == 0) {
                    files.push(action.payload.file);
                } else {
                    files[0] = action.payload.file;
                }
            } else {
                files.push(action.payload.file);
            }

            return {
                ...state,
                uploadState: {
                    files: files,
                    loading: true,
                    error: null
                }
            }
        case UPLOAD_FILE:
            return {
                ...state,
                uploadState: {
                    files: [],
                    loading: true,
                    error: null
                }
            };
        case UPLOAD_FILE_SUCCESS:
            return {
                ...state,
                uploadState: {
                    loading: false,
                    error: null,
                    files: []
                }
            };
        case UPLOAD_FILE_FAILURE:
            error = action.payload || {
                message: action.payload.message
            };
            return {
                ...state,
                uploadState: {
                    files: [],
                    error: error,
                    loading: false
                }
            };
        case REMOVE_FILE:
            return {
                ...state,
                uploadState: {
                    files: state.uploadState.files.filter(el => el != action.payload),
                    loading: true,
                    error: null
                }
            }
        default:
            return state;
    }
}