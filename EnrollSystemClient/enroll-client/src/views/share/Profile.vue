<template>
  <v-container id="user-profile" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12" md="8">
        <material-card>
          <template v-slot:heading>
            <div class="text-h3 font-weight-light">Profile</div>
          </template>
          <v-form>
            <v-container class="py-0">
              <v-row>
                <v-col cols="12" md="4">
                  <v-text-field
                    v-if="currentUser.role === 'admin'"
                    :value="currentUser.userName"
                    label="Tên đăng nhập"
                    readonly
                  />
                  <v-text-field
                    v-else
                    :value="currentUser.userName"
                    label="MSSV"
                    readonly
                  />
                </v-col>
                <v-col cols="12" md="8">
                  <v-text-field
                    v-model="currentUser.name"
                    class="purple-input"
                    label="Họ và tên"
                  />
                </v-col>
                <v-col cols="12" md="8">
                  <v-text-field
                    value="*********"
                    class="purple-input"
                    label="Mật khẩu"
                    readonly
                  />
                </v-col>
                <v-col cols="12" md="4">
                  <v-dialog v-model="changePasswordDialog" max-width="500px">
                    <template v-slot:activator="{ on, attrs }">
                      <v-btn color="success" v-bind="attrs" v-on="on">
                        Đổi mật khẩu
                      </v-btn>
                    </template>
                    <v-card>
                      <v-card-title>
                        <span class="headline">Đổi mật khẩu</span>
                      </v-card-title>
                      <v-divider></v-divider>
                      <v-card-text>
                        <v-container>
                          <v-row>
                            <v-col cols="12">
                              <v-text-field
                                v-model="currentPassword"
                                label="Mật khẩu hiện tại"
                                required
                                type="password"
                                name="currentPassword"
                              ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                              <v-text-field
                                v-model="newPassword"
                                label="Mật khẩu mới"
                                required
                                type="password"
                                name="newPassword"
                              ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                              <v-text-field
                                v-model="passwordConfirm"
                                label="Xác nhận mật khẩu mới"
                                required
                                type="password"
                                name="passwordConfirm"
                              ></v-text-field>
                            </v-col>
                          </v-row>
                        </v-container>
                      </v-card-text>
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="success" @click="changePassword()">
                          Thay đổi
                        </v-btn>
                        <v-btn
                          color="error"
                          @click="changePasswordDialogHide()"
                        >
                          Hủy
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-dialog>
                </v-col>
                <v-col cols="12" md="4">
                  <v-text-field
                    label="Giới tính"
                    class="purple-input"
                    :value="gender"
                    readonly
                  />
                </v-col>
                <v-col cols="12" md="8">
                  <v-text-field
                    label="Số CMND"
                    v-model="currentUser.idNumber"
                    class="purple-input"
                  />
                </v-col>
                <v-col cols="12" md="12">
                  <v-text-field
                    label="Địa chỉ"
                    v-model="currentUser.address"
                    class="purple-input"
                  />
                </v-col>
                <v-col cols="12" class="text-right">
                  <v-btn @click="updateProfile()" color="success" class="mr-0">
                    Cập nhật
                  </v-btn>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </material-card>
      </v-col>

      <v-col cols="12" md="4">
        <material-card v-if="avatar" class="v-card-profile" :avatar="avatar">
          <v-col cols="12" class="text-center">
            <!-- <v-btn small color="success" rounded class="mr-0">
              Đổi ảnh đại diện
            </v-btn> -->
            <v-dialog v-model="changeAvatarDialog" max-width="500px">
              <template v-slot:activator="{ on, attrs }">
                <v-btn color="success" v-bind="attrs" v-on="on">
                  Đổi ảnh đại diện
                </v-btn>
              </template>
              <v-card>
                <v-card-title>
                  <span class="headline">Đổi ảnh đại diện</span>
                </v-card-title>
                <v-divider></v-divider>
                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12" class="text-center">
                        <v-avatar
                          v-if="newAvatar"
                          size="128"
                          class="mx-auto elevation-6"
                        >
                          <v-img :src="newAvatar" />
                        </v-avatar>
                      </v-col>
                      <v-col cols="12">
                        <v-file-input
                          v-model="currentFile"
                          :rules="rules"
                          accept="image/png, image/jpeg, image/bmp"
                          placeholder="Chọn ảnh đại diện"
                          prepend-icon="mdi-camera"
                          label="Chọn ảnh đại diện"
                          :clearable="false"
                          @change="selectFile"
                        ></v-file-input>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="success" @click="changeAvatar()">
                    Thay đổi
                  </v-btn>
                  <v-btn color="error" @click="changeAvatarDialogHide">
                    Hủy
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-col>
          <v-card-text class="text-center">
            <h6 class="text-h4 md-1 grey--text">
              {{ currentUser.name }}
            </h6>

            <h4 class="text-h5 font-weight-light md-3 black--text">
              {{ role }}
            </h4>
          </v-card-text>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../components/base/MaterialCard.vue";
import * as API from "../../constant/api";
import UserService from "../../services/user.service.js";
export default {
  name: "Profile",
  data() {
    return {
      avatar: "",
      currentFile: undefined,
      newAvatar: "",
      currentPassword: "",
      newPassword: "",
      passwordConfirm: "",
      currentUser: {},
      items: [
        { name: "Nam", value: true },
        { name: "Nữ", value: false },
      ],
      changePasswordDialog: false,
      changeAvatarDialog: false,
      rules: [
        (value) =>
          !value ||
          value.size < 3000000 ||
          "Kích thước Avatar phải nhỏ hơn 3MB",
      ],
    };
  },
  components: {
    MaterialCard,
  },
  computed: {
    // avatar() {
    //   return `${API.SERVER}/${this.currentUser.avatar}`;
    // },
    gender() {
      if (this.currentUser.gender) {
        return "Nam";
      }
      return "Nữ";
    },
    role() {
      if (this.currentUser.role === "admin") {
        return "Giáo Vụ";
      }
      if (this.currentUser.role === "teacher") {
        return "Giáo Viên";
      }
      return "Học Viên";
    },
  },
  methods: {
    selectFile() {
      this.newAvatar = URL.createObjectURL(this.currentFile);
    },
    changeAvatarDialogHide() {
      this.changeAvatarDialog = false;
      this.newAvatar = "";
      this.currentFile = null;
    },
    changePasswordDialogHide() {
      this.changePasswordDialog = false;
      this.currentPassword = "";
      this.newPassword = "";
      this.passwordConfirm = "";
    },
    changePassword() {
      let formData = new FormData();
      formData.append("password", this.newPassword);
      formData.append("confirmPassword", this.passwordConfirm);
      formData.append("gender", this.currentUser.gender);
      UserService.editUser(this.currentUser.id, formData).then(
        (response) => {
          console.log(response);
          this.changePasswordDialog = false;
          this.currentPassword = "";
          this.newPassword = "";
          this.passwordConfirm = "";
        },
        (error) => {
          console.log(error);
        }
      );
    },
    changeAvatar() {
      let formData = new FormData();
      if (this.currentFile != null) {
        formData.append("image", this.currentFile);
        formData.append("gender", this.currentUser.gender);
        UserService.editUser(this.currentUser.id, formData).then(
          (response) => {
            console.log(response.data);
            this.avatar = this.newAvatar;
            this.changeAvatarDialog = false;
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    updateProfile() {
      let formData = new FormData();
      formData.append('name', this.currentUser.name);
      formData.append('gender', this.currentUser.gender);
      formData.append('idNumber', this.currentUser.idNumber);
      formData.append('address', this.currentUser.address);
      UserService.editUser(this.currentUser.id, formData).then(
          (response) => {
            console.log(response.data);
          },
          (error) => {
            console.log(error);
          }
        );
    },
    getProfile() {
      UserService.getProfile().then(
        (response) => {
          this.currentUser = response.data;
          this.avatar = `${API.SERVER}/${this.currentUser.avatar}`;
        },
        (error) => {
          console.log(error);
        }
      );
    },
  },
  mounted() {
    this.getProfile();
  },
};
</script>