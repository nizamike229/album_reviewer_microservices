<script lang="ts">
  import axios from "axios";
  import { onMount } from "svelte";
  import { goto } from "$app/navigation";
  import { getUsername } from "../lib/apiClient";

  let query = "";
  let albums: string | any[] = [];
  let loading = false;

  type Album = {
    title: string;
    artist: string;
    mbid: string;
    cover: string;
  };

  type MusicBrainzItem = {
    id: string;
    title: string;
    "artist-credit"?: Array<{ name: string }>;
  };

  async function search() {
    albums = [];
    loading = true;

    if (!query.trim()) {
      loading = false;
      return;
    }

    try {
      const res = await axios.get(
        "https://musicbrainz.org/ws/2/release-group/",
        {
          params: {
            query: `release:${query}`,
            type: "album",
            fmt: "json",
          },
          headers: {
            "User-Agent": "YourApp/1.0 (youremail@example.com)",
          },
        }
      );

      const items = res.data["release-groups"];

      const coversLoaded = items.map(async (item: MusicBrainzItem) => {
        const mbid = item.id;
        const coverUrl = `https://coverartarchive.org/release-group/${mbid}/front`;
        try {
          await axios.head(coverUrl);
          return {
            title: item.title,
            artist: item["artist-credit"]?.[0]?.name ?? "Unknown",
            mbid,
            cover: coverUrl,
          };
        } catch {
          return null;
        }
      });

      albums = (await Promise.all(coversLoaded)).filter(Boolean);
    } catch (err) {
      console.error("Ошибка при поиске:", err);
    } finally {
      loading = false;
    }
  }

  const handleRedirectToAlbum = (mbid: string) => {
    window.location.href = `/album/${mbid}`;
  };

  function isLoggedIn() {
    return document.cookie.includes("access_token");
  }

  onMount(() => {
    if (!isLoggedIn()) {
      goto("/promo");
    }
  });
</script>

<header class="w-full fixed top-0 left-0 z-30 h-20 flex items-center justify-between px-4 md:px-12 shadow-xl border-b-2 border-white bg-zinc-900 backdrop-blur-md">
  <span class="text-2xl font-bold text-indigo-400">🎧 Music Albums</span>
  <div class="flex items-center gap-4">
    <span class="text-xl font-semibold text-zinc-100 font-mono"
      >{getUsername() || "Гость"}</span
    >
    <img
      src="/default-avatar.jpg"
      alt="Аватар"
      class="w-16 h-16 rounded-xl bg-zinc-800 border border-zinc-700 shadow"
    />
  </div>
</header>
<div class="pt-24 min-h-screen bg-zinc-900 text-zinc-100 px-4 py-8 font-mono">
  <h1 class="text-4xl font-bold mb-8 text-center font-mono">
    🎧 Поиск альбомов
  </h1>

  <div class="max-w-3xl mx-auto mb-10">
    <input
      type="text"
      bind:value={query}
      on:keydown={(e: KeyboardEvent) => e.key === "Enter" && search()}
      placeholder="Введите название альбома…"
      class="w-full bg-zinc-800 text-white text-xl px-5 py-4 rounded-2xl outline-none focus:ring-2 focus:ring-zinc-400 transition font-mono"
    />
  </div>

  <div
    class="max-w-6xl mx-auto min-h-[200px] font-mono grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-10"
  >
    {#if loading}
      <div class="flex justify-center items-center py-20 col-span-full">
        <div
          class="animate-spin inline-block w-12 h-12 border-4 border-zinc-500 border-t-zinc-200 rounded-full"
        ></div>
      </div>
    {:else if albums.length === 0 && query}
      <p class="text-center text-zinc-400 col-span-full">
        Ничего не найдено или нет обложек.
      </p>
    {:else}
      {#each albums as album (album.mbid)}
        <!-- svelte-ignore a11y_click_events_have_key_events -->
        <!-- svelte-ignore a11y_no_static_element_interactions -->
        <button
          class="bg-zinc-800 hover:bg-zinc-700 rounded-3xl p-8 shadow-xl transition border-2 border-zinc-700 font-mono flex flex-col items-center w-full h-[400px] min-h-[400px] max-h-[420px] min-w-[280px] max-w-[400px] mx-auto cursor-pointer focus:ring-2 focus:ring-yellow-400 duration-200 group"
          on:click={handleRedirectToAlbum.bind(null, album.mbid)}
        >
          <img
            src={album.cover}
            alt="Обложка"
            class="w-48 h-48 object-cover rounded-2xl shadow-lg mb-6 transition-transform duration-200 group-hover:scale-105 group-hover:shadow-2xl"
          />
          <h2
            class="text-2xl font-semibold text-zinc-100 font-mono text-center truncate w-full mb-2"
          >
            {album.title}
          </h2>
          <p
            class="text-zinc-400 text-lg font-mono text-center truncate w-full"
          >
            {album.artist}
          </p>
        </button>
      {/each}
    {/if}
  </div>
</div>
