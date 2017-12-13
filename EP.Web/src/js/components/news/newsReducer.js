import {
    CREATE_NEWS,
    CREATE_NEWS_FAILURE,
    CREATE_NEWS_SUCCESS,
    DELETE_NEWS,
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS
} from './newsActions';

const INITIAL_STATE = {
    createState: {
        id: null,
        loading: false,
        error: null
    },
    listState: {
        page: 1,
        items: [],
        loading: false,
        error: null
    }
};

export default function(state = INITIAL_STATE, action) {
    let error;
    switch (action.type) {
        case CREATE_NEWS:
            return {
                ...state,
                createState: {
                    id: null,
                    loading: true,
                    error: null
                }
            };
        case CREATE_NEWS_SUCCESS:
            return {
                ...state,
                createState: {
                    id: action.payload,
                    loading: false,
                    error: null
                }
            };
        case CREATE_NEWS_FAILURE:
            error = action.payload || {
                message: action.payload.message
            }
            return {
                ...state,
                createState: {
                    id: null,
                    error: error,
                    loading: false
                }
            };
        case GET_NEWS_LIST:
            return {
                ...state,
                listState: {
                    items: [],
                    loading: true,
                    error: null
                }
            };
        case GET_NEWS_LIST_SUCCESS:
            return {
                ...state,
                listState: {
                    items: action.payload.items ? action.payload.items : [],
                    loading: false,
                    error: null
                }
            };
        case GET_NEWS_LIST_FAILURE:
            error = action.payload || {
                message: action.payload.message
            }
            return {
                ...state,
                listState: {
                    items: [],
                    error: error,
                    loading: false
                }
            };
        default:
            return state;
    }
}