import {
    createAction
} from 'redux-actions';

import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_LIST_FAILURE,
    GOTO_CREATE_NEWS,
    INIT_CREATE_NEWS
} from './types';

import {
    NewsFilter, NewsFetchSuccess, NewsFetchFail
} from './models';

export const getNews = createAction<NewsFilter>(GET_NEWS_LIST);
export const getNewsSuccess = createAction<NewsFetchSuccess>(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction<NewsFetchFail>(GET_NEWS_LIST_FAILURE);
export const gotoCreateNews = createAction(GOTO_CREATE_NEWS);
export const initCreateNews = createAction(INIT_CREATE_NEWS);
