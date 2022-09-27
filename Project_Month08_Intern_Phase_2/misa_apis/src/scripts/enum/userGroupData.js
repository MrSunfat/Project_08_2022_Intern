const userGroupData = [
    {
        UserGroupID: 2,
        UserGroupName: 'Cán bộ thuộc Cục Thu thập và đơn vị sự nghiệp',
        TotalMember: 5,
        Description:
            'Nhóm toàn bộ CBCCVC thuộc Cục Thu thập và các đơn vị sự nghiệp tham gia đề nghị đi công tác của chuyến công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: null,
        ModifiedDate: null,
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: 2,
    },
    {
        UserGroupID: 1,
        UserGroupName: 'Cán bộ thuộc đơn vị hành chính (trừ Cục Thu thập)',
        TotalMember: 28,
        Description:
            'Nhóm toàn bộ CBCCVC thuộc các đơn vị hành chính (trừ Cục Thu thập) tham gia đề nghị đi công tác, đề nghị tạm ứng, đề nghị thanh toán của chuyến đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: null,
        ModifiedDate: null,
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: 1,
    },
    {
        UserGroupID: 13,
        UserGroupName: 'Lãnh đạo Tổng cục phê duyệt đề nghị công tác',
        TotalMember: 2,
        Description:
            'Nhóm gồm các Lãnh đạo tổng cục tham gia phê duyệt đề nghị đi công tác (dành cho quy trình phê duyệt chuyến công tác không có lãnh đạo Vụ tham gia trong đoàn)',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: null,
        ModifiedDate: null,
        EditVersion: '2022-08-22T09:56:06.322+07:00',
        IsSystem: 1,
        SortOrder: 7,
    },
    {
        UserGroupID: 3,
        UserGroupName: 'Lãnh đạo đơn vị phê duyệt đề nghị công tác',
        TotalMember: 5,
        Description:
            'Nhóm gồm các Thủ trưởng đơn vị (Vụ trưởng/Viện trưởng/...) thuộc Tổng cục tham gia phê duyệt đề nghị đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:55:02.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 8,
        UserGroupName: 'Phê duyệt đề nghị tạm ứng',
        TotalMember: 1,
        Description:
            'Lãnh đạo Văn phòng Tổng cục (Chánh Văn phòng) tham gia phê duyệt đề nghị tạm ứng',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: null,
        ModifiedDate: null,
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: 9,
    },
    {
        UserGroupID: 6,
        UserGroupName: 'Phó Tổng cục trưởng phê duyệt đề nghị công tác',
        TotalMember: 2,
        Description:
            'Nhóm gồm các Phó tổng cục trưởng tham gia phê duyệt đề nghị đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T13:50:04.000+07:00',
        EditVersion: '2022-08-12T21:01:23.762+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 12,
        UserGroupName: 'Quản lý xe',
        TotalMember: 2,
        Description:
            'Trưởng phòng xe tham gia thực hiện xếp lái xe cho đoàn công tác (Người dùng trong nhóm này có quyền thao tác trong mục Phương tiện đi)',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:54:33.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 9,
        UserGroupName: 'Thực hiện chi tạm ứng',
        TotalMember: 3,
        Description:
            'Nhóm gồm các Chuyên viên phòng Tài vụ - VPTC tham gia chi tạm ứng',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:53:36.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 11,
        UserGroupName: 'Thực hiện thanh toán',
        TotalMember: 3,
        Description:
            'Kế toán trưởng thuộc phòng Tài vụ - VPTC thực hiện thanh toán cho người đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:53:57.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 7,
        UserGroupName: 'Tiếp nhận đề nghị tạm ứng',
        TotalMember: 3,
        Description:
            'Nhóm gồm các Chuyên viên phòng Tài vụ - VPTC tham gia tiếp nhận đề nghị tạm ứng',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:25:14.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 10,
        UserGroupName: 'Tiếp nhận đề nghị thanh toán',
        TotalMember: 3,
        Description:
            'Kế toán trưởng thuộc phòng Tài vụ - VPTC tham gia phê duyệt và tính chi phí công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T15:53:47.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 4,
        UserGroupName: 'Tiếp nhận đề nghị đi công tác',
        TotalMember: 3,
        Description:
            'Nhóm gồm các CBCCVC thuộc phòng Tổng hợp Thư ký - VPTC tiếp nhận đề nghị đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T10:43:50.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
    {
        UserGroupID: 5,
        UserGroupName: 'Tổng cục trưởng phê duyệt đề nghị công tác',
        TotalMember: 1,
        Description: 'Tổng cục trưởng tham gia phê duyệt đề nghị đi công tác',
        Status: 1,
        TenantID: '4b4306dd-3644-4519-92cc-30f2eb614ef0',
        CreatedBy: null,
        CreatedDate: '2022-08-25T16:28:08.000+07:00',
        ModifiedBy: '',
        ModifiedDate: '2022-08-26T13:53:16.000+07:00',
        EditVersion: '2022-08-11T08:51:37.838+07:00',
        IsSystem: 1,
        SortOrder: null,
    },
];

export default userGroupData;
