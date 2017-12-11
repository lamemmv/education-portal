import {
    FETCH_POSTS, FETCH_POSTS_SUCCESS, FETCH_POSTS_FAILURE, RESET_POSTS,
    CREATE_POST, CREATE_POST_SUCCESS, CREATE_POST_FAILURE, RESET_NEW_POST
} from './postActions';

const INITIAL_STATE = {
    postsList: {
        files: [],
        showDeleteConfirmation: false,
        fileTobeDeleted: null,
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
        case FETCH_POSTS:// start fetching posts and set loading = true
            return { ...state, postsList: { files: [], error: null, loading: true } };
        case FETCH_POSTS_SUCCESS:// return list of posts and make loading = false
            let pages = [], showPagination = false;
            if (action.payload.totalPages > 1) {
                for (let i = 0; i < action.payload.totalPages; i++) {
                    pages.push(i + 1);
                }
                showPagination = true;
            }
            return {
                ...state, postsList: {
                    files: action.payload,
                    pages: pages,
                    showPagination: showPagination,
                    error: null,
                    loading: false
                }
            };
        case FETCH_POSTS_FAILURE:// return error and make loading = false
            error = action.payload || { message: action.payload.message };//2nd one is network or server down errors
            return { ...state, postsList: { files: [], error: error, loading: false } };
        case RESET_POSTS:// reset postList to initial state
            return { ...state, postsList: { files: [], error: null, loading: false } };
        default:
            return state;
    }
}