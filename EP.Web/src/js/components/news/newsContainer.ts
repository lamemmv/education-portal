import {
    connect
} from 'react-redux';

import {
    Dispatch, bindActionCreators
} from 'redux';

import * as FetchNewsActions from './newsActions';

import {
    getNews,
    getNewsSuccess,
    getNewsFailure
} from './newsActions';

import { NewsModel, NewsFetchSuccess, NewsFetchFail, NewsFilter } from './models';

import NewsList from './list';

const mapStateToProps = (state: NewsModel) => {
    return {
        news: state
    };
}

const mapDispatchToProps = (dispatch: Dispatch<{}>) => {
    return {
        actions: bindActionCreators(FetchNewsActions as any, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NewsList);