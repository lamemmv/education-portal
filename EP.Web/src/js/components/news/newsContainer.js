import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import * as FetchNewsActions from './newsActions';

import NewsList from './list';

const mapStateToProps = (state) => {
    return {
        news: state.news
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(FetchNewsActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NewsList);