import {
    createAction
} from 'redux-actions';

import {
    NewsFilter, NewsFetchSuccess, NewsFetchFail
} from './models';

export const CREATE_NEWS = 'CREATE_NEWS';
export const CREATE_NEWS_SUCCESS = 'CREATE_NEWS_SUCCESS';
export const CREATE_NEWS_FAILURE = 'CREATE_NEWS_FAILURE';
export const GET_NEWS_LIST = 'GET_NEWS_LIST';
export const GET_NEWS_LIST_SUCCESS = 'GET_NEWS_LIST_SUCCESS';
export const GET_NEWS_LIST_FAILURE = 'GET_NEWS_LIST_FAILURE';
export const DELETE_NEWS = 'DELETE_NEWS';
export const DELETE_NEWS_SUCCESS = 'DELETE_NEWS_SUCCESS';
export const DELETE_NEWS_FAILURE = 'DELETE_NEWS_FAILURE';

export const getNews = createAction<NewsFilter>(GET_NEWS_LIST);
export const getNewsSuccess = createAction<NewsFetchSuccess>(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction<NewsFetchFail>(GET_NEWS_LIST_FAILURE);
