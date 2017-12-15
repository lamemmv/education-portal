import {
    createAction
} from 'redux-actions';

import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_LIST_FAILURE
} from './types';

import {
    NewsFilter, NewsFetchSuccess, NewsFetchFail
} from './models';

export const getNews = createAction<NewsFilter>(GET_NEWS_LIST);
export const getNewsSuccess = createAction<NewsFetchSuccess>(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction<NewsFetchFail>(GET_NEWS_LIST_FAILURE);
