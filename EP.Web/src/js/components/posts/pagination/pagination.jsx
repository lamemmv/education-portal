import React, { Component } from "react";
import { render } from 'react-dom';
import { Table, Menu, Icon } from 'semantic-ui-react';

class Pagination extends Component {

    render() {
        const {
            currentPage,
            pages
        } = this.props.fileState;
        const { getFiles } = this.props;

        return (
            <Table.Footer>
                <Table.Row>
                    <Table.HeaderCell colSpan='3'>
                        <Menu floated='right' pagination>
                            <Menu.Item as='a' icon
                                onClick={() => getFiles(currentPage - 1)}
                                disabled={currentPage <= 1}>
                                <Icon name='left chevron' />
                            </Menu.Item>
                            {
                                pages.map(page => (
                                    <Menu.Item as='a' key={page} onClick={() => getFiles(page)}
                                        active={page == currentPage}>{page}
                                    </Menu.Item>
                                ))
                            }
                            <Menu.Item as='a' icon
                                onClick={() => getFiles(currentPage + 1)}
                                disabled={currentPage >= pages.length}>
                                <Icon name='right chevron' />
                            </Menu.Item>
                        </Menu>
                    </Table.HeaderCell>
                </Table.Row>
            </Table.Footer>
        );
    }
}

export default Pagination;