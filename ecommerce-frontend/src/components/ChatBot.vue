<template>
  <div class="chat-widget">
    <v-btn
      icon="mdi-robot"
      color="deep-purple-accent-3"
      size="x-large"
      elevation="8"
      class="chat-toggle-btn"
      @click="toggleChat"
    ></v-btn>

    <v-scale-transition>
      <v-card v-if="isOpen" class="chat-window" elevation="12">
        <v-toolbar color="deep-purple-darken-3" density="compact">
          <v-toolbar-title class="text-subtitle-1 font-weight-bold">
            <v-icon start>mdi-creation</v-icon> Asistent Virtual
          </v-toolbar-title>
          <v-btn icon="mdi-close" size="small" @click="toggleChat"></v-btn>
        </v-toolbar>

        <v-card-text class="chat-messages" ref="messagesContainer">
          <div v-if="messages.length === 0" class="text-center text-grey mt-4">
            <v-icon size="40" color="grey-lighten-2">mdi-message-text-outline</v-icon>
            <p class="text-caption mt-2">Salut! Cu ce te pot ajuta azi? <br> Îți pot recomanda laptopuri sau telefoane.</p>
          </div>

          <div
            v-for="(msg, index) in messages"
            :key="index"
            :class="['d-flex mb-3', msg.role === 'user' ? 'justify-end' : 'justify-start']"
          >
            <v-sheet
              :color="msg.role === 'user' ? 'deep-purple-lighten-5' : 'grey-lighten-4'"
              class="pa-3 rounded-lg"
              max-width="80%"
            >
              <div class="text-body-2" style="white-space: pre-wrap;">{{ msg.text }}</div>
            </v-sheet>
          </div>

          <div v-if="isLoading" class="d-flex justify-start mb-3">
            <v-sheet color="grey-lighten-4" class="pa-3 rounded-lg">
              <v-progress-circular indeterminate color="deep-purple" size="20"></v-progress-circular>
            </v-sheet>
          </div>
        </v-card-text>

        <v-divider></v-divider>
        <div class="pa-2 d-flex align-center bg-white">
          <v-text-field
            v-model="userInput"
            placeholder="Scrie o întrebare..."
            variant="solo-filled"
            density="compact"
            hide-details
            rounded="xl"
            @keyup.enter="sendMessage"
            :disabled="isLoading"
          >
            <template v-slot:append-inner>
              <v-btn
                icon="mdi-send"
                variant="text"
                color="deep-purple"
                size="small"
                @click="sendMessage"
                :disabled="!userInput || isLoading"
              ></v-btn>
            </template>
          </v-text-field>
        </div>
      </v-card>
    </v-scale-transition>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: "ChatBot",
  data() {
    return {
      isOpen: false,
      isLoading: false,
      userInput: "",
      messages: [] // Aici stocam istoric: { role: 'user'/'ai', text: '...' }
    };
  },
  methods: {
    toggleChat() {
      this.isOpen = !this.isOpen;
      // Scroll la fund cand deschizi
      if (this.isOpen) {
        this.$nextTick(() => this.scrollToBottom());
      }
    },
    async sendMessage() {
      if (!this.userInput.trim()) return;

      const question = this.userInput;
      
      // 1. Adaugam mesajul utilizatorului in lista
      this.messages.push({ role: 'user', text: question });
      this.userInput = "";
      this.isLoading = true;
      this.scrollToBottom();

      try {
        // 2. Trimitem la Backend
        // ATENTIE: Verifica portul (8080 sau 9090)
        const response = await axios.post('http://localhost:8080/api/ai/ask', question, {
          headers: { 'Content-Type': 'text/plain' }
        });

        // 3. Adaugam raspunsul AI in lista
        this.messages.push({ role: 'ai', text: response.data });

      } catch (error) {
        console.error("Eroare Chatbot:", error);
        this.messages.push({ role: 'ai', text: "Îmi pare rău, nu pot comunica cu serverul momentan." });
      } finally {
        this.isLoading = false;
        this.scrollToBottom();
      }
    },
    scrollToBottom() {
      // Auto-scroll ca sa vezi ultimul mesaj
      const container = this.$refs.messagesContainer;
      if (container) {
        // Folosim setTimeout mic pentru a lasa DOM-ul sa se randeze
        setTimeout(() => {
          container.$el.scrollTop = container.$el.scrollHeight;
        }, 50);
      }
    }
  }
};
</script>

<style scoped>
/* Pozitionare fixa in dreapta jos */
.chat-widget {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
}

.chat-toggle-btn {
  margin-top: 10px;
}

.chat-window {
  width: 350px;
  height: 500px;
  display: flex;
  flex-direction: column;
  border-radius: 16px;
  overflow: hidden;
  margin-bottom: 10px;
}

.chat-messages {
  flex-grow: 1;
  overflow-y: auto;
  background-color: #f5f5f5;
}
</style>