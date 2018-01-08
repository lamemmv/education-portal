import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import { connect } from 'preact-redux';
import { Localizer, Text } from 'preact-i18n';
import API from '../api';

import {
    getFiles
} from './fileActions';

import { askForDeleting } from './delete/actions';

import FileMenu from './menu/container';
import Pagination from './pagination/container';
import DeleteFile from './delete/container';
import NotificationContainer from '../notify/notification.container';
import * as styles from './styles.css';

class FileList extends Component {

    componentWillReceiveProps(nextProps, prevProps) {
        if (nextProps.match.params) {
            if (nextProps.match.params.id != this.props.match.params.id) {
                this.init(nextProps.match.params.id);
            }
        }
    }

    componentWillMount() {
        if (this.props.match.params.id != null) {
            this.init(this.props.match.params.id);
        } else {
            this.init();
        }
    }

    init = (folderId) => {
        if (!this.props.fileState.files) {
            this.props.fileState.files = [];
        }
        this.props.getFiles({
            page: this.props.fileState.currentPage,
            folderId: folderId
        });
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination />
            ) : null
        );
    }

    renderNode = (node) => {
        const imageTypes = ['image/gif', "image/jpeg", "image/png"];
        let nodeItem = (
            <div class='card clearfix'>
                {
                    imageTypes.indexOf(node.contentType) < 0 ?
                        <img class="img-fluid card-img-top"
                            src={require('../../../assets/images/480px-Icons8_flat_folder.png')} />
                        :
                        <img class="img-fluid card-img-top"
                            src={API.getServerDomain() + node.virtualPath} />
                }
                <div class='card-block'>
                    <h4 class='card-title'>{node.name}</h4>
                </div>
            </div>);

        if (node.nodeType == 1) { // folder
            return (
                <div class="col-2 ep-node-item"><Link to={`/files/${node.id}`}>{nodeItem}</Link></div>
            );
        } else {
            return (nodeItem);
        }
    }

    render() {
        const {
            files,
            currentPage,
            pages,
            showPagination,
            blob
        } = this.props.fileState;

        const imageTypes = ['image/gif', "image/jpeg", "image/png"];
        return (
            <section class='container'>
                <NotificationContainer />
                {this.props.match.params.id ? <FileMenu params={blob} /> : null}
                <div class='row'>
                    {
                        files.map((node) => {
                            return this.renderNode(node)
                        })
                    }
                </div>
            </section>
        );
    };
}

const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getFiles: (filter) => {
            dispatch(getFiles(filter));
        },
        askForDeleting: (id) => {
            dispatch(askForDeleting(id));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(FileList);
