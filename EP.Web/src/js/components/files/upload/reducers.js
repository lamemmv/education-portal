import {
    UPLOAD_FILE,
    UPLOAD_FILE_FAILURE,
    UPLOAD_FILE_SUCCESS,
    SELECTED_FILE,
    BROWSE_FILE,
    REMOVE_FILE
} from '../types';

const INITIAL_STATE = {
    uploadState: {
        files: [],
        error: null,
        loading: false
    }
};

export default function(state = INITIAL_STATE, action) {
    let error;
    switch (action.type) {
        case SELECTED_FILE:
            let files = state.uploadState.files.slice();
            const _files = Array.from(action.payload.files);
            if (action.payload.single) {
                if (files.length == 0) {
                    files = files.concat(_files);
                } else {
                    files = _files;
                }
            } else {
                files = files.concat(_files);
            }

            return {
                ...state,
                uploadState: {
                    files: files,
                    parent: action.payload.parent,
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
                    response: action.payload,
                    browseFile: true,
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
            };
        case BROWSE_FILE:
            return {
                ...state,
                uploadState: {
                    files: [],
                    parent: action.payload,
                    browseFile: true,
                    loading: true,
                    error: null
                }
            };
        default:
            return state;
    }
}