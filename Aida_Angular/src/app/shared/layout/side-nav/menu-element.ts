export const menus = [
    {
        'name': 'Dashboard',
        'icon': { iconName: 'tachometer-alt', prefix: 'fas' } ,
        'link': '/secured/dashboards/company',
        'open': false,
        'color':'parent-menu-color',
        'sub': []
    },
    {
        'name': 'Client View',
        'icon': { iconName: 'street-view', prefix: 'fas' } ,
        'link': '/secured/dashboards/client-view',
        'open': false,
        'color':'parent-menu-color',
        'sub': []
    },
    {
        'name': 'Masters',
        'icon': { iconName: 'window-restore', prefix: 'fas' },
        'link': false,
        'open': false,
        'color':'parent-menu-color',
        'sub': [
            {
                'name': 'Company Profile',
                'icon': { iconName: 'building', prefix: 'fas' },
                'link': '/secured/masters/company',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Employee',
                'icon': { iconName: 'user-tie', prefix: 'fas' },
                'link': '/secured/masters/employee',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Document',
                'icon': { iconName: 'file-invoice', prefix: 'fas' },
                'link': '/secured/masters/document',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Service SOP Definition',
                'icon': { iconName: 'sitemap', prefix: 'fas' },
                'link': '/secured/masters/service-definition',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Client Profile Maintenance',
                'icon': { iconName: 'business-time', prefix: 'fas' },
                'link': '/secured/masters/client-profile',
                'open': false,
                'color':'child-menu-color',
            },
        ]

    },
    {
        'name': 'Secretarial Service',
        'icon': { iconName: 'user-graduate', prefix: 'fas' },
        'link': false,
        'open': false,
        'color':'parent-menu-color',
        'sub': [
            {
                'name': 'Registration',
                'icon': { iconName: 'registered', prefix: 'fas' },
                'link': '/secured/services/clients/registrations',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Post Registration',
                'icon': { iconName: 'id-card-alt', prefix: 'fas' },
                'link': '/secured/services/clients/post-registration',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'AGM - AR',
                'icon': { iconName: 'mail-bulk', prefix: 'fas' },
                'link': '/secured/services/clients/agm-ar',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Change Resolution',
                'icon': { iconName: 'exchange-alt', prefix: 'fas' },
                'link': '/secured/services/clients/change-resolution',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Appointment',
                'icon': { iconName: 'user-plus', prefix: 'fas' },
                'link': '/secured/services/clients/appointment',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Resignation',
                'icon': { iconName: 'user-minus', prefix: 'fas' },
                'link': '/secured/services/clients/resignation',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Share Capital',
                'icon': { iconName: 'coins', prefix: 'fas' },
                'link': '/secured/services/clients/share-capital',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Strike Off',
                'icon': { iconName: 'strikethrough', prefix: 'fas' },
                'link': '/secured/services/clients/strike-off',
                'open': false,
                'color':'child-menu-color',
            }

        ]
    },
    {
        'name': 'Accounting Service',
        'icon': { iconName: 'calculator', prefix: 'fas' } ,
        'link': '/secured/services/clients/accounting',
        'open': false,
        'color':'parent-menu-color',
    },
    {
        'name': 'Download Forms',
        'icon': { iconName: 'file-download', prefix: 'fas' } ,
        'link': false,
        'open': false,
        'color':'parent-menu-color',
        'sub': [
            {
                'name': 'Secretarial Common',
                'icon': { iconName: 'file-contract', prefix: 'fas' },
                'link': '/secured/masters/download-forms/secretarial-common',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Agreements',
                'icon': { iconName: 'handshake', prefix: 'fas' },
                'link': '/secured/masters/download-forms/agreements',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Post Registration',
                'icon': { iconName: 'id-card-alt', prefix: 'fas' },
                'link': '/secured/masters/download-forms/post-registration',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Share Capital',
                'icon': { iconName: 'coins', prefix: 'fas' },
                'link': '/secured/masters/download-forms/share-capital',
                'open': false,
                'color':'child-menu-color',
            },
            {
                'name': 'Strike Off',
                'icon': { iconName: 'strikethrough', prefix: 'fas' },
                'link': '/secured/masters/download-forms/strike-0ff',
                'open': false,
                'color':'child-menu-color',
            }
        ]
    },
    {
        'name': 'Reports',
        'icon': { iconName: 'scroll', prefix: 'fas' } ,
        'link': false,
        'open': false,
        'color':'parent-menu-color',
        'sub': [
            {
                'name': 'Services Summary',
                'icon': { iconName: 'poll', prefix: 'fas' },
                'link': '/secured/reports/services-summary',
                'open': false,
                'color':'child-menu-color',
            }
        ]
    }          
    
];