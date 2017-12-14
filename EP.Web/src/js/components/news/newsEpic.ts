import {
    GET_NEWS_LIST,
    getNewsSuccess,
    getNewsFailure
} from './newsActions';

import API from '../api';
import 'rxjs';
import { Observable } from 'rxjs/Observable';
import { Epic } from 'redux-observable';

const fetchNewsEpic: Epic<any, any> = (action$, store) =>
    action$.ofType(GET_NEWS_LIST)
        .mergeMap((action: any) =>
            Observable.fromPromise(API.getNews(action.page))
                .map(response => getNewsSuccess(response.data))
                .catch(error => Observable.of(getNewsFailure(error)))
        )

export default fetchNewsEpic;