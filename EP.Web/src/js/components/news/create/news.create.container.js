import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import * as NewsActions from '../newsActions';

import NewsCreate from './news.create';

const mapStateToProps = (state) => {
    return {
        news: state.newsCreate
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(NewsActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NewsCreate);