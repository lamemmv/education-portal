import { h, Component } from "preact";
import { Link } from 'react-router-dom';
import UploadIndex from './upload/index';
import Pagination from './pagination/index';
import DeleteFile from './delete/index';

class FileList extends Component {

    componentWillMount() {
        this.props.getFiles(this.props.fileState.currentPage);
    }

    renderTableFooter(pages, currentPage, showPagination) {
        return (
            showPagination ? (
                <Pagination />
            ) : null
        );
    }

    renderTableBody(files) {
        const { askForDeleting } = this.props;
        return (
            // files.map((file) => {
            //     return (<Table.Row key={file.id}>
            //         <Table.Cell>
            //             {file.fileName}
            //         </Table.Cell>
            //         <Table.Cell>
            //             <Link to={`/files/${file.id}`}>
            //                 <Image avatar src={require('../../../assets/images/image.png')} />
            //             </Link>
            //         </Table.Cell>
            //         <Table.Cell>
            //             <Button onClick={() => askForDeleting(file.id)}>Delete</Button>
            //         </Table.Cell>
            //     </Table.Row>)
            // })
            <div>body</div>
        );
    }

    render() {
        const {
            files,
            currentPage,
            pages,
            showPagination
        } = this.props.fileState;
        const { askForDeleting } = this.props;
        return (
            // <Container >
            //     <UploadIndex />
            //     <Table color={color} key={color}>
            //         <Table.Header>
            //             <Table.Row>
            //                 <Table.HeaderCell>File name</Table.HeaderCell>
            //                 <Table.HeaderCell>Preview</Table.HeaderCell>
            //                 <Table.HeaderCell></Table.HeaderCell>
            //             </Table.Row>
            //         </Table.Header>
            //         <Table.Body>
            //             {this.renderTableBody(files)}
            //         </Table.Body>
            //         {this.renderTableFooter(pages, currentPage, showPagination)}
            //     </Table>
            //     <DeleteFile />
            // </Container>
            <div>
                <UploadIndex />
                <div class="mdc-grid-list mdc-grid-list--with-icon-align-start">
                    <ul class="mdc-grid-list__tiles">
                        {files.map((file) => {
                            return (
                                <li class="mdc-grid-tile">
                                    <div class="mdc-grid-tile__primary">
                                        <img class="mdc-grid-tile__primary-content"
                                            src={require('../../../assets/images/image.png')} />
                                    </div>
                                    <span class="mdc-grid-tile__secondary">
                                        <i class="mdc-grid-tile__icon material-icons"
                                            onClick={() => askForDeleting(file.id)}>clear</i>
                                        <span class="mdc-grid-tile__title">{file.fileName}</span>
                                    </span>
                                </li>
                            )
                        })
                        }
                    </ul>
                </div>
                <DeleteFile />
            </div>
        );
    };
}

export default FileList;
