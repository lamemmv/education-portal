import {
    GET_FILES,
    GET_FILES_SUCCESS,
    GET_FILES_FAILURE
} from './types';

let isfile = (node) => {
    return node.contentType && node.virtualPath;
}

const INITIAL_STATE = {
    fileState: {
        files: [],
        showPagination: false,
        pages: [],
        currentPage: 1,
        error: null,
        loading: false
    }
};

export default function (state = INITIAL_STATE, action) {
    let error;
    switch (action.type) {
        case GET_FILES: // start fetching files and set loading = true
            return {
                ...state,
                fileState: {
                    files: [],
                    pages: [],
                    currentPage: action.payload,
                    error: null,
                    loading: true
                }
            };
        case GET_FILES_SUCCESS: // return list of files and make loading = false
            let pages = [],
                showPagination = false;
            if (action.payload.totalPages > 1) {
                for (let i = 0; i < action.payload.totalPages; i++) {
                    pages.push(i + 1);
                }
                showPagination = true;
            }

            let files = [];
            if (action.payload) {
                action.payload.map((node) => {
                    files.push(Object.assign({}, node, {
                        nodeType: !isfile(node) ? 1 : 2 // 1: folder, 2: file
                    }));
                })
            }
            return {
                ...state,
                fileState: {
                    currentPage: action.payload.page,
                    files: files,
                    pages: pages,
                    showPagination: showPagination,
                    error: null,
                    loading: false
                }
            };
        case GET_FILES_FAILURE: // return error and make loading = false
            error = action.payload || {
                message: action.payload.message
            }; //2nd one is network or server down errors
            return {
                ...state,
                fileState: {
                    files: [],
                    pages: [],
                    error: error,
                    loading: false
                }
            };
        default:
            return state;
    }
}