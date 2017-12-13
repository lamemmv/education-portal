import React, { Component } from "react";
import { render } from 'react-dom';
import { HashRouter, NavLink, Link } from 'react-router-dom';
import {
    Container, Divider, Dropdown, Header, Image, Menu, Visibility,
} from 'semantic-ui-react';

import logo from '../../assets/images/logo.png';
// The Header creates links that can be used to navigate
// between routes.
class EPHeader extends Component {
    constructor(props, context) {
        super(props, context);
    }

    state = {
        menuFixed: false,
        overlayFixed: false,
    }

    stickTopMenu = () => this.setState({ menuFixed: true })

    unStickOverlay = () => this.setState({ overlayFixed: false })

    render() {
        return (
            <HashRouter>
                <Visibility
                    onBottomPassed={this.stickTopMenu}
                    onBottomVisible={this.unStickTopMenu}
                    once={false} >
                    <Menu
                        borderless
                        fixed={'top'}>
                        <Container text>
                            <Menu.Item>
                                <Image size='mini' src='/logo.png' />
                            </Menu.Item>
                            <Menu.Item as={Link} to="/">Home</Menu.Item>
                            <Menu.Item as={Link} to='/files'>Files</Menu.Item>
                            <Menu.Item as={Link} to='/news'>News</Menu.Item>

                            <Menu.Menu position='right'>
                                <Dropdown text='Dropdown' pointing className='link item'>
                                    <Dropdown.Menu>
                                        <Dropdown.Item>List Item</Dropdown.Item>
                                        <Dropdown.Item>List Item</Dropdown.Item>
                                        <Dropdown.Divider />
                                        <Dropdown.Header>Header Item</Dropdown.Header>
                                        <Dropdown.Item>
                                            <i className='dropdown icon' />
                                            <span className='text'>Submenu</span>
                                            <Dropdown.Menu>
                                                <Dropdown.Item>List Item</Dropdown.Item>
                                                <Dropdown.Item>List Item</Dropdown.Item>
                                            </Dropdown.Menu>
                                        </Dropdown.Item>
                                        <Dropdown.Item>List Item</Dropdown.Item>
                                    </Dropdown.Menu>
                                </Dropdown>
                            </Menu.Menu>
                        </Container>
                    </Menu>
                </Visibility>
            </HashRouter>
        );
    };
}

export default EPHeader;
