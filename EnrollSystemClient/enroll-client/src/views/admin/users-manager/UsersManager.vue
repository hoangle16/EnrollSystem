<template>
  <v-container id="user-manager" fluid tab="section">
    <v-row justify="center">
      <v-col cols="12" md="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Quản lý người dùng</div>
          </template>
          <v-data-table
            :headers="headers"
            :items="users"
            :search="search"
            :items-per-page="15"
            class="elevation-1"
            :loading="isLoading"
            loading-text="Đang tải dữ liệu..."
            :footer-props="{
              'items-per-page-text': 'Số cột mỗi trang',
            }"
          >
            <template v-slot:top>
              <v-row class="p-3">
                <v-col cols="12" sm="6" md="4">
                  <v-text-field
                    dense
                    v-model="search"
                    append-icon="mdi-magnify"
                    label="Tìm kiếm"
                    hide-details
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                  <v-select
                    v-model="roleFilter"
                    :items="roleItems"
                    label="Vài trò"
                    dense
                    clearable
                    @change="filterByRole"
                  ></v-select>
                </v-col>
                <v-spacer></v-spacer>
                <v-dialog v-model="newUserDialog" max-width="500px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn
                      small
                      color="success"
                      dark
                      class="mb-2"
                      v-bind="attrs"
                      v-on="on"
                    >
                      <v-icon class="mr-2">mdi-account-plus</v-icon>
                      Thêm
                    </v-btn>
                  </template>
                  <v-card>
                    <v-card-title>
                      <span class="headline"> Người dùng mới </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.userName"
                              label="Tên đăng nhập"
                              name="userName"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.password"
                              label="Mật khẩu"
                              name="password"
                              type="password"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.confirmPassword"
                              label="Nhập lại mật khẩu"
                              name="confirmPassword"
                              type="password"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.name"
                              label="Họ tên"
                              name="name"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-select
                              v-model="newUser.gender"
                              :items="genderItems"
                              label="Giới tính"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.idNumber"
                              label="CMND"
                              name="idNumber"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.phoneNumber"
                              label="Số điện thoại"
                              name="phoneNumber"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newUser.address"
                              label="Địa chỉ"
                              hint="Phường/xã, quận/huyện, tỉnh/thành phố"
                              name="address"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-select
                              v-model="newUser.role"
                              :items="roleItems"
                              label="Vai trò"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="createUser">Thêm</v-btn>
                      <v-btn color="error" @click="newUserDialogHide"
                        >Hủy</v-btn
                      >
                    </v-card-actions>
                  </v-card>
                </v-dialog>
                <!--Delete User Dialog -->
                <v-dialog persistent v-model="deleteDialog" max-width="500px">
                  <v-card>
                    <v-card-title>
                      <span class="headline"
                        >Bạn có chắc muốn xóa người dùng này?</span
                      >
                    </v-card-title>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="confirmDelete()"
                        >Xóa</v-btn
                      >
                      <v-btn color="error" @click="deleteDialogHide()">
                        Hủy</v-btn
                      >
                      <v-spacer></v-spacer>
                    </v-card-actions>
                  </v-card>
                </v-dialog>
                <!-- Edit User Dialog -->
                <v-dialog v-model="editUserDialog" max-width="500px">
                  <v-card>
                    <v-card-title>
                      <span class="headline"> Cập nhật thông tin </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.userName"
                              label="Tên đăng nhập"
                              name="userName"
                              disabled
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.password"
                              label="Mật khẩu"
                              name="password"
                              type="password"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.confirmPassword"
                              label="Nhập lại mật khẩu"
                              name="confirmPassword"
                              type="password"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.name"
                              label="Họ tên"
                              name="name"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-select
                              v-model="selectedUser.gender"
                              :items="genderItems"
                              label="Giới tính"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.idNumber"
                              label="CMND"
                              name="idNumber"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.phoneNumber"
                              label="Số điện thoại"
                              name="phoneNumber"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedUser.address"
                              label="Địa chỉ"
                              hint="Phường/xã, quận/huyện, tỉnh/thành phố"
                              name="address"
                            ></v-text-field>
                          </v-col>
                          <v-col cols="12">
                            <v-select
                              v-model="selectedUser.role"
                              :items="roleItems"
                              label="Vai trò"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="confirmEditUser"
                        >Cập nhật</v-btn
                      >
                      <v-btn color="error" @click="editUserDialogHide"
                        >Hủy</v-btn
                      >
                    </v-card-actions>
                  </v-card>
                </v-dialog>
              </v-row>
            </template>
            <!-- custom data table columns -->
            <template v-slot:[`item.actions`]="{ item }">
              <v-icon small class="mr-2" @click="editUser(item)"
                >mdi-pencil</v-icon
              >
              <v-icon small @click="deleteUser(item)">mdi-delete</v-icon>
            </template>
            <template v-slot:[`item.gender`]="{ item }">
              <div v-if="item.gender == true">Nam</div>
              <div v-else>Nữ</div>
            </template>
            <template v-slot:[`item.role`]="{ item }">
              <div v-if="item.role == 'admin'">Giáo vụ</div>
              <div v-else-if="item.role == 'teacher'">Giáo viên</div>
              <div v-else>Học viên</div>
            </template>
            <template v-slot:[`item.isActive`]="{ item }">
              <v-switch
                dense
                color="success"
                v-model="item.isActive"
                @change="blockUser(item)"
              ></v-switch>
            </template>
          </v-data-table>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../components/base/MaterialCard.vue";
//import * as API from '../../../constant/api';
import UserService from "../../../services/user.service.js";
export default {
  name: "UserManager",
  components: {
    MaterialCard,
  },
  data() {
    return {
      roleFilter: "",
      search: "",
      isLoading: false,
      deleteDialog: false,
      newUserDialog: false,
      editUserDialog: false,
      headers: [
        {
          text: "Tên đăng nhập",
          align: "start",
          sortable: true,
          value: "userName",
        },
        { text: "Họ tên", value: "name", align: "start" },
        { text: "Giới tính", width: 100, value: "gender", align: "start" },
        { text: "CMND", value: "idNumber", align: "start" },
        { text: "SĐT", value: "phoneNumber", align: "start" },
        { text: "Vai trò", width: 100, value: "role", align: "start" },
        {
          text: "Kích hoạt",
          sortable: false,
          width: 100,
          value: "isActive",
          align: "start",
        },
        { text: "", width: 90, value: "actions", sortable: false },
      ],
      originalUsers: [],
      users: [],
      newUser: {
        userName: "",
        password: "",
        confirmPassword: "",
        name: "",
        gender: true,
        idNumber: "",
        phoneNumber: "",
        address: "",
        role: "",
      },
      selectedUser: {},
      genderItems: [
        {
          text: "Nam",
          value: true,
        },
        {
          text: "Nữ",
          value: false,
        },
      ],
      roleItems: [
        {
          text: "Giáo vụ",
          value: "admin",
        },
        {
          text: "Giáo viên",
          value: "teacher",
        },
        {
          text: "Học viên",
          value: "student",
        },
      ],
    };
  },
  computed: {},
  methods: {
    getUsers() {
      this.isLoading = true;
      UserService.getUsers().then(
        (response) => {
          this.originalUsers = response.data;
          this.users = this.originalUsers;
          //console.log(this.users);
          this.isLoading = false;
        },
        (error) => {
          console.log(error);
          this.isLoading = false;
        }
      );
    },
    newUserDialogHide() {
      this.newUserDialog = false;
    },
    editUserDialogHide() {
      this.editUserDialog = false;
      this.selectedUser = {};
    },
    deleteDialogHide() {
      this.deleteDialog = false;
      this.selectedUser = {};
    },
    createUser() {
      console.log(this.newUser);
      this.newUser.isActive = true;
      UserService.createUser(this.newUser).then(
        (response) => {
          console.log(response.data);
          this.newUserDialog = false;
          this.$toast("Tạo người dùng mới thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.newUser = {};
          this.getUsers();
        },
        (error) => {
          console.log(error);
        }
      );
    },
    editUser(item) {
      console.log("edit", item);
      this.selectedUser = item;
      this.editUserDialog = true;
    },
    confirmEditUser() {
      if (this.selectedUser != null) {
        console.log(this.selectedUser);
        let formData = new FormData();
        formData.append("password", this.selectedUser.password);
        formData.append("confirmPassword", this.selectedUser.confirmPassword);
        formData.append("name", this.selectedUser.name);
        formData.append("gender", this.selectedUser.gender);
        formData.append("idNumber", this.selectedUser.idNumber);
        formData.append("phoneNumber", this.selectedUser.phoneNumber);
        formData.append("address", this.selectedUser.address);
        formData.append("role", this.selectedUser.role);
        UserService.editUser(this.selectedUser.id, formData).then(
          (response) => {
            console.log(response);
            this.$toast("Cập nhật người dùng thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.editUserDialogHide();
            this.getUsers();
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    deleteUser(item) {
      this.selectedUser = item;
      this.deleteDialog = true;
    },
    confirmDelete() {
      if (this.selectedUser != null) {
        UserService.deleteUser(this.selectedUser.id).then(
          (response) => {
            console.log(response.data);
            this.$toast("Xóa người dùng thành công", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.deleteDialogHide();
            this.getUsers();
            this.selectedUser = {};
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    filterByRole() {
      if (this.roleFilter != null) {
        this.users = this.originalUsers.filter(
          (user) => user.role === this.roleFilter
        );
      } else {
        this.users = this.originalUsers;
      }
    },
    blockUser(item) {
      UserService.blockUser(item.id).then(
        (response) => {
          console.log(response.data);
        },
        (error) => {
          console.log(error);
        }
      );
    },
  },
  mounted() {
    this.getUsers();
  },
};
</script>

<style lang="scss" scoped>
.v-data-table-header th {
  white-space: nowrap;
}
</style>