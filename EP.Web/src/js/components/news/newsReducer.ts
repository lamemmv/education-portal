import { handleActions, Action } from 'redux-actions';
import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS
} from './types';

import { NewsItem, NewsModel, NewsFetchSuccess, NewsFetchFail, NewsFilter } from './models';

const initialState: NewsModel = <NewsModel>{
    items: [],
    page: 1,
    loading: false,
    error: null
};

export default handleActions<NewsModel, any>({
    [GET_NEWS_LIST]: (state: NewsModel, action: Action<NewsFilter>) => {
        return (<any>Object).assign({}, state, {
            items: [],
            page: action.payload.page,
            error: null,
            loading: true
        });
    },
    [GET_NEWS_LIST_SUCCESS]: (state: NewsModel, action: Action<NewsFetchSuccess>) => {
        return (<any>Object).assign({}, state, {
            items: action.payload.items,
            page: action.payload.page,
            error: null,
            loading: true
        });
    },
    [GET_NEWS_LIST_FAILURE]: (state: NewsModel, action: Action<NewsFetchFail>) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return (<any>Object).assign({}, state, {
            items: [],
            page: action.payload.page,
            error: error,
            loading: true
        });
    },

}, initialState);